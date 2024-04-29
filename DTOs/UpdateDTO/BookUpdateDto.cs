namespace BackendBookstore.DTOs.UpdateDTO
{
    public class BookUpdateDto
    {
        public int BookId { get; set; }

        public string BookTitle { get; set; } = null!;

        public string BookAuthor { get; set; } = null!;

        public DateOnly PublishingYear { get; set; }

        public string Publisher { get; set; } = null!;

        public decimal BookPrice { get; set; }

        public int Available { get; set; }

        public int? CategoryId { get; set; }
    }
}
