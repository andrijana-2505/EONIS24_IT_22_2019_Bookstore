using AutoMapper;
using BackendBookstore.DTOs.CreateDTO;
using BackendBookstore.DTOs.ReadDTO;
using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendBookstore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class OrderItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderItemRepo _repository;

        public OrderItemController(IOrderItemRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<OrderItemReadDto>> GetAll(int? orderId)
        {
            var items = _repository.GetOrderItems(orderId);
            if (items == null || !items.Any())
                return NoContent();
            return Ok(_mapper.Map<IEnumerable<OrderItemReadDto>>(items));
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{orderItemId}", Name = "GetOrderItemById")]
        public ActionResult<OrderItemReadDto> GetOrderItemById(int orderItemId)
        {
            Orderitem item = _repository.FindOrderItemById(orderItemId);
            if (item != null)
                return Ok(_mapper.Map<OrderItemReadDto>(item));
            return NotFound();

        }

        //[Authorize(Roles = "Admin, Customer")]
        [HttpPost]
        public ActionResult<OrderItemReadDto> CreateOrderItem(OrderItemCreateDto item)
        {
            var itemModel = _mapper.Map<Orderitem>(item);
            try
            {
                _repository.Create(itemModel);
                _repository.SaveChanges();
                var itemDto = _mapper.Map<OrderItemUpdateDto>(itemModel);
                return _mapper.Map<OrderItemReadDto>(itemModel);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
            }

        }
        //[Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult<OrderItemReadDto> Update(OrderItemUpdateDto order)
        {
            try
            {
                var oldOrder = _repository.FindOrderItemById(order.OrderItemId);
                if (oldOrder == null)
                {
                    return NotFound();
                }
                Orderitem orderEntity = _mapper.Map<Orderitem>(order);
                _mapper.Map(orderEntity, oldOrder);
                _repository.SaveChanges();
                return Ok(_mapper.Map<OrderItemReadDto>(oldOrder));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{orderItemId}")]
        public IActionResult Delete(int orderItemId)
        {
            try
            {
                var item = _repository.FindOrderItemById(orderItemId);
                if (item == null)
                {
                    return NotFound();
                }
                _repository.Delete(orderItemId);
                _repository.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
    }
}
