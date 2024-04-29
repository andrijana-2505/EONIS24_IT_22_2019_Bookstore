namespace BackendBookstore.DTOs.CreateDTO
{
    public class OrderItemCreateDto
    {
        public int? Quantity { get; set; }

        public int? OrdersId { get; set; }

        public int? BookId { get; set; }
    }
}
