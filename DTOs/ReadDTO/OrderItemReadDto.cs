using BackendBookstore.DTOs.UpdateDTO;

namespace BackendBookstore.DTOs.ReadDTO
{
    public class OrderItemReadDto
    {
        public int OrderItemId { get; set; }

        public int? Quantity { get; set; }

        public int? OrdersId { get; set; }

        public int? BookId { get; set; }
        public virtual ICollection<OrderUpdateDto> Orders { get; set; } = new List<OrderUpdateDto>();
        public virtual ICollection<BookUpdateDto> Books { get; set; } = new List<BookUpdateDto>();

    }
}
