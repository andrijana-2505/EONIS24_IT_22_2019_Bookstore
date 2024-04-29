using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;

namespace BackendBookstore.DTOs.ReadDTO
{
    public class OrderReadDto
    {
        public int OrdersId { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        public DateOnly? OrderDate { get; set; }

        public string? StripeTransactionId { get; set; }

        public int? UsersId { get; set; }

        public virtual ICollection<AddressUpdateDto> Addresses { get; set; } = new List<AddressUpdateDto>();

        public virtual ICollection<OrderItemUpdateDto> Orderitems { get; set; } = new List<OrderItemUpdateDto>();
    }
}
