using BackendBookstore.DTOs;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Address = BackendBookstore.Models.Address;

namespace BackendBookstore.Controllers
{
    [ApiController]
    public class CheckoutApiController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IConfiguration _config;
        private readonly IAddressRepo _address;
        public CheckoutApiController(IOrderRepo orderRepo, IConfiguration config, IAddressRepo address)
        {
            _orderRepo = orderRepo;
            _config = config;
            _address = address;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }
        [HttpPost]
        [Route("api/create-checkout-session")]
        public ActionResult CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
        {
            List<LineItemsDto> orderItems = request.OrderItems;
            int orderId = request.OrderId;
            string street = request.Street;
            string city = request.City;
            string postalCode = request.PostalCode;

            var service = new SessionService();

            var options = new SessionCreateOptions
            {
                LineItems = orderItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = item.UnitAmount,
                        Currency = "rsd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                        },
                    },
                    Quantity = item.Quantity,
                }).ToList(),

                Mode = "payment",
                SuccessUrl = _config["frontend_url"] + "/success",
                CancelUrl = _config["frontend_url"] + "/cart",
                Metadata = new Dictionary<string, string> // Add metadata here
                {
                    { "OrderId", orderId.ToString() },
                    { "Street", street },
                    { "City", city },
                    { "PostalCode", postalCode }
                }

            };


            Session session = service.Create(options);

            return Ok(session.Url);

        }

        const string endpointSecret = "whsec_52d86b9bfda5829f14b54e6bf350238e9ace1b4228f58c2dfcf7912e24425d73";

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                Console.WriteLine("webhook verified");
                // Handle the event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = (Stripe.Checkout.Session)stripeEvent.Data.Object;

                    // Access metadata
                    int orderId = int.Parse(session.Metadata["OrderId"]);

                    Address addressModel = new Address();

                    addressModel.Street = session.Metadata["Street"];
                    addressModel.City = session.Metadata["City"];
                    addressModel.PostalCode = session.Metadata["PostalCode"];
                    var addressId = 0;
                    try
                    {
                        _address.Create(addressModel);
                        _address.SaveChanges();

                        addressId = addressModel.AddressId;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    var oldOrder = _orderRepo.FindOrderById(orderId);
                    if (oldOrder == null)
                    {
                        return NotFound();
                    }
                    oldOrder.Status = OrderStatus.Obrada;
                    //oldOrder.AddressId = addressId;
                    _orderRepo.SaveChanges();

                    try
                    {

                        Order uplata = new Order();
                        uplata.OrdersId = orderId;
                        uplata.StripeTransactionId = session.PaymentIntentId;
                        uplata.TotalAmount = session.AmountTotal / 100;

                        _orderRepo.Create(uplata);
                        _orderRepo.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }



                    return Ok();

                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("webhook not working");
                return BadRequest();
            }
        }
    }
    public class CreateCheckoutSessionRequest
    {
        public List<LineItemsDto> OrderItems { get; set; }
        public int OrderId { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

    }
}

