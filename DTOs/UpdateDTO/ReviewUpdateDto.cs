namespace BackendBookstore.DTOs.UpdateDTO
{
    public class ReviewUpdateDto
    {
        public int ReviewId { get; set; }

        public int Rating { get; set; }

        public DateOnly? ReviewDate { get; set; }

        public int? UsersId { get; set; }

        public int? BookId { get; set; }
    }
}
