namespace BackendBookstore.DTOs.CreateDTO
{
    public class OrderCreateDto
    {
        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        public DateOnly? OrderDate { get; set; }

        public string? StripeTransactionId { get; set; }

        public int? UsersId { get; set; }
    }
}
