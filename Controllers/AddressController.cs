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
    public class AddressController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAddressRepo _repository;
        public AddressController(IMapper mapper, IAddressRepo repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<AddressReadDto>> GetAll()
        {
            var addresses = _repository.GetAddresses();
            if (addresses == null || !addresses.Any())
                return NoContent();
            var addressDtos = _mapper.Map<IEnumerable<AddressReadDto>>(addresses);
            foreach (var addressDto in addressDtos)
            {
                var address = _repository.FindAddressById(addressDto.AddressId);
                var orderDto = _mapper.Map<IEnumerable<OrderUpdateDto>>(address.Orders);
                addressDto.Orders = orderDto.ToList();
            }
            return Ok(addressDtos);
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("{addressId}", Name = "GetAddressById")]
        public ActionResult<AddressReadDto> GetAddressById(int addressId)
        {
            Address address = _repository.FindAddressById(addressId);
            if (address != null)
            {
                var order = _mapper.Map<IEnumerable<OrderUpdateDto>>(address.Orders);
                var addressDto = _mapper.Map<AddressReadDto>(address);
                addressDto.Orders = order.ToList();
                return Ok(addressDto);
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpPost]
        public ActionResult<AddressReadDto> CreateAddress(AddressCreateDto address)
        {
            var addressModel = _mapper.Map<Address>(address);
            try
            {
                _repository.Create(addressModel);
                _repository.SaveChanges();
                var addressDto = _mapper.Map<AddressUpdateDto>(addressModel);
                return CreatedAtRoute(nameof(GetAddressById), new { addressId = addressDto.AddressId }, addressDto);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult<AddressReadDto> Update(AddressUpdateDto address)
        {
            try
            {
                var oldAddress = _repository.FindAddressById(address.AddressId);
                if (oldAddress == null)
                {
                    return NotFound();
                }
                Address addressEntity = _mapper.Map<Address>(address);
                _mapper.Map(addressEntity, oldAddress);
                _repository.SaveChanges();
                return Ok(_mapper.Map<AddressReadDto>(oldAddress));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpDelete("{addressId}")]
        public IActionResult Delete(int addressId)
        {
            try
            {
                var address = _repository.FindAddressById(addressId);
                if (address == null)
                {
                    return NotFound();
                }
                _repository.Delete(addressId);
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
