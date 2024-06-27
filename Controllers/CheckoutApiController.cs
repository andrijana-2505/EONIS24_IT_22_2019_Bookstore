using BackendBookstore.DTOs;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Implementation;
using BackendBookstore.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Address = BackendBookstore.Models.Address;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BackendBookstore.Controllers
{
    [ApiController]
    public class CheckoutApiController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IConfiguration _config;
        private readonly IAddressRepo _addressRepo;
        private readonly IOrderItemRepo _oRepo;
        private readonly IUserRepo _userRepo;

        public CheckoutApiController(IOrderRepo orderRepo, IConfiguration config, IAddressRepo addressRepo, IOrderItemRepo oRepo, IUserRepo userRepo)
        {
            _orderRepo = orderRepo;
            _config = config;
            _addressRepo = addressRepo;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            _oRepo = oRepo;
            _userRepo = userRepo;
        }

        [HttpPost]
        [Route("api/create-checkout-session")]
        public ActionResult CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
        {
            var orderItems = request.OrderItems;
            var orderId = request.OrderId;
            var street = request.Street;
            var city = request.City;
            var postalCode = request.PostalCode;
            var customer = request.Customer;



            var service = new SessionService();

            var options = new SessionCreateOptions
            {
                LineItems = request.OrderItems.Select(item => new SessionLineItemOptions
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
                SuccessUrl = "http://127.0.0.1:5173/success",
                CancelUrl = "http://127.0.0.1:5173/cart",

                Metadata = new Dictionary<string, string>
                {
                    { "orderId", orderId.ToString() },
                    { "street", street },
                    { "city", city },
                    { "postalCode", postalCode },
                    { "LineItems", JsonConvert.SerializeObject(orderItems) },
                    { "customer", customer }

                }

              
        };
            Console.WriteLine(options.Metadata);
            Session session = service.Create(options);

            return Ok(new { id = session.Id });
        }

        const string endpointSecret = "whsec_1bd4aeca98addcd21535b54b23240342a5fa23491a109e2fbcbac3b3c32890c3";


        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Console.WriteLine(json);
            var signatureHeader = Request.Headers["Stripe-Signature"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = (Stripe.Checkout.Session)stripeEvent.Data.Object;
                    Console.WriteLine(session);
                    // Extracting the metadata
                    var metadata = session.Metadata;

                    string street = metadata["street"];
                    string city = metadata["city"];
                    string postalCode = metadata["postalCode"];
                    var lineItemsJson = metadata["LineItems"];
                    var orderItems = JsonConvert.DeserializeObject<List<LineItemsDto>>(lineItemsJson);
                    string customer = metadata["customer"];

                

                    var order = new Order {
                        TotalAmount = session.AmountSubtotal / 100,
                        Status = OrderStatus.Obrada,
                        OrderDate = DateOnly.FromDateTime(DateTime.Now),
                        StripeTransactionId = session.Id,
                        UsersId = Int32.Parse(customer)
                    };
                    //var order = _orderRepo.FindOrderById()
                    

                    int orderId = _orderRepo.CreateId(order);

                    // Assuming you have methods to fetch the original order details using orderId
                    var address = new Address
                    {
                        Street = street,
                        City = city,
                        PostalCode = postalCode,
                        OrdersId = orderId
                    };

                    await _addressRepo.AddAsync(address);

                    foreach (var item in orderItems)
                    {
                        var orderItem = new Orderitem
                        {
                            Quantity = item.Quantity,
                            OrdersId = orderId,
                            BookId = item.BookId
                        };
                        _oRepo.Create(orderItem);
                        // Save order item to the database
                    }

                    return Ok();
                }

                return Ok();
            }
            catch (StripeException e)
            {
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
        public string Customer { get; set; }
    }
}
