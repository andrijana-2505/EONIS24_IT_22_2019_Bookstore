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
                        UnitAmount = item.UnitAmount * 100,
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

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var endpointSecret = "whsec_1bd4aeca98addcd21535b54b23240342a5fa23491a109e2fbcbac3b3c32890c3";
            var signatureHeader = Request.Headers["Stripe-Signature"];

            // Log the Stripe-Signature header
            Console.WriteLine($"Stripe-Signature: {signatureHeader}");

            
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);
                
                Console.WriteLine("Received webhook event");
                Console.WriteLine("Webhook event verified: " + stripeEvent.Type);

                // Handle the event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = (Stripe.Checkout.Session)stripeEvent.Data.Object;
                    Console.WriteLine("Checkout session completed: " + session.Id);

                }
                else
                {
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Stripe exception: {e}");
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

