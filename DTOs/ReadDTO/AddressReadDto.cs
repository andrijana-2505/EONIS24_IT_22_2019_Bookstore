using BackendBookstore.DTOs.UpdateDTO;

namespace BackendBookstore.DTOs.ReadDTO
{
    public class AddressReadDto
    {
        public int AddressId { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostalCode { get; set; } = null!;
        public int? OrdersId { get; set; }
        public virtual ICollection<OrderUpdateDto> Orders { get; set; } = new List<OrderUpdateDto>();


    }
}
