using AutoMapper;
using BackendBookstore.DTOs;
using BackendBookstore.DTOs.ReadDTO;
using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace BackendBookstore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepo _repository;
        private readonly IOrderRepo _orderRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        public UserController(IUserRepo repository, IMapper mapper, IOrderRepo orderRepo, IAddressRepo addressRepo, IOrderItemRepo orderItem)
        {
            _orderItemRepo = orderItem;
            _repository = repository;
            _addressRepo = addressRepo;
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetAll(UserRole? userRole)
        {
            var users = _repository.GetUsers(userRole);
            if (users == null || !users.Any())
                return NoContent();
            var usersDtos = _mapper.Map<IEnumerable<UserReadDto>>(users);
            foreach (var user in usersDtos)
            {
                user.Orders = _mapper.Map<IEnumerable<OrderUpdateDto>>(_repository.GetOrdersForUser(user.UsersId)).ToList();
                user.Reviews = _mapper.Map<IEnumerable<ReviewUpdateDto>>(_repository.GetReviewsForUser(user.UsersId)).ToList();
            }
            return Ok(usersDtos);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("id/{userId}", Name = "GetUserById")]
        public ActionResult<UserReadDto> GetUserById(int userId)
        {
            User user = _repository.FindUserById(userId);
            if (user != null)
            {
                var order = _mapper.Map<IEnumerable<OrderUpdateDto>>(user.Orders);
                var userDto = _mapper.Map<UserReadDto>(user);
                userDto.Orders = order.ToList();
                return Ok(userDto);
            }
            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult<UserReadDto> UpdateUser(UserUpdateDto usersUpdateDto)
        {
            try
            {
                var user = _repository.FindUserById(usersUpdateDto.UsersId);

                if (user == null)
                {
                    return NotFound();
                }

                // Update the user entity with the new data
                user.FirstName = usersUpdateDto.FirstName;
                user.LastName = usersUpdateDto.LastName;
                user.Phone = usersUpdateDto.Phone;
                user.UserRole = (UserRole)usersUpdateDto.UserRole;


                _repository.SaveChanges();

                return Ok(_mapper.Map<UserReadDto>(user));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            try
            {
                var user = _repository.FindUserById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                _repository.Delete(userId);
                _repository.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpPut("my-data")]
        public ActionResult<UserReadDto> Update(UserUpdateDto user)
        {
            try
            {
                //getting an email from a user
                var currentUser = _repository.FindByEmail(User?.Identity?.Name);
                var oldUser = _repository.FindUserById(currentUser.UsersId);
                if (oldUser == null)
                {
                    return NotFound();
                }

                oldUser.FirstName = user.FirstName;
                oldUser.LastName = user.LastName;
                oldUser.Phone = user.Phone;
                _repository.SaveChanges();
                return Ok(_mapper.Map<UserReadDto>(oldUser));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("my-data")]
        public ActionResult<UserReadDto> GetMyData()
        {
            try
            {
                User user = _repository.FindByEmail(User?.Identity?.Name);
                return Ok(_mapper.Map<UserReadDto>(user));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error loading data");
            }

        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpPatch("my-data/password")]
        public ActionResult UpdatePassword([FromBody] PasswordUpdateDto passwordUpdate)
        {
            try
            {
                var oldUser = _repository.FindByEmail(User?.Identity?.Name);
                Console.WriteLine(oldUser.Email);
                if (!BCrypt.Net.BCrypt.Verify(passwordUpdate.OldPassword, oldUser.PasswordLogin))
                    return BadRequest("Wrong password for user");
                else
                {
                    oldUser.PasswordLogin = BCrypt.Net.BCrypt.HashPassword(passwordUpdate.NewPassword);
                    _repository.SaveChanges();
                    return Ok("Password successfully changed");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("orders")]
        public ActionResult<IEnumerable<OrderReadDto>> GetMyOrders(int? orderId)
        {
            var user = _repository.FindByEmail(User?.Identity?.Name);
            var orders = _repository.GetOrdersForUser(user.UsersId).ToList();
            var ordersDtoList = new List<OrderReadDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderReadDto>(order);
                orderDto.Orderitems = _mapper.Map<IEnumerable<OrderItemUpdateDto>>(
                    _orderRepo.GetOrderItemsForOrder(orderDto.OrdersId)).ToList();

                ordersDtoList.Add(orderDto);
            }


            return ordersDtoList;
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpPost("last_order")]
        public OrderReadDto GetOrderInProgress()
        {
            var user = _repository.FindByEmail(User?.Identity?.Name);
            var existingOrder = _orderRepo.GetOrderInProgressForUser(user.UsersId);

            if (existingOrder != null)
            {
                var inprogress = _mapper.Map<OrderReadDto>(existingOrder);
                inprogress.Orderitems = _mapper.Map<IEnumerable<OrderItemUpdateDto>>(
                    _orderRepo.GetOrderItemsForOrder(inprogress.OrdersId)).ToList();
                return _mapper.Map<OrderReadDto>(inprogress);
            }
            else
            {
                // Create a new order for the user with status 'u_Procesu'
                var newOrder = new Order
                {
                    UsersId = user.UsersId,
                    Status = OrderStatus.U_procesu,
                };

                _orderRepo.Create(newOrder);
                _orderRepo.SaveChanges();

                // Return the newly generated order ID
                return _mapper.Map<OrderReadDto>(newOrder);
            }
        }
        
        [Authorize(Roles = "Admin, Customer")]
        [HttpDelete("orderitems/{orderItemId}")]
        public IActionResult DeleteOrderItem(int orderItemId)
        {
            var user = _repository.FindByEmail(User?.Identity?.Name);
            var existingOrder = _orderRepo.GetOrderInProgressForUser(user.UsersId);

            if (existingOrder == null)
            {
                return NotFound("No existing order found for the user.");
            }

            var existingOrderItem = _orderItemRepo.FindOrderItemById(orderItemId);

            if (existingOrderItem == null || existingOrderItem.OrdersId != existingOrder.OrdersId)
            {
                return NotFound("Order item not found or does not belong to the user's existing order.");
            }

            _orderItemRepo.Delete(existingOrderItem.OrderItemId);
            _orderItemRepo.SaveChanges();

            return NoContent();
        }
    }
}
