namespace BackendBookstore.DTOs.UpdateDTO
{
    public class OrderItemUpdateDto
    {
        public int OrderItemId { get; set; }

        public int? Quantity { get; set; }

        public int? OrdersId { get; set; }

        public int? BookId { get; set; }
    }
}
