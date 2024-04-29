namespace BackendBookstore.DTOs.CreateDTO
{
    public class ReviewCreateDto
    {
        public int Rating { get; set; }

        public DateOnly? ReviewDate { get; set; }

        public int? UsersId { get; set; }

        public int? BookId { get; set; }
    }
}
