using BackendBookstore.DTOs.UpdateDTO;

namespace BackendBookstore.DTOs.ReadDTO
{
    public class ReviewReadDto
    {
        public int ReviewId { get; set; }

        public int Rating { get; set; }

        public DateOnly? ReviewDate { get; set; }

        public int? UsersId { get; set; }

        public int? BookId { get; set; }
        public virtual ICollection<UserUpdateDto> Users { get; set; } = new List<UserUpdateDto>();
        public virtual ICollection<BookUpdateDto> Books { get; set; } = new List<BookUpdateDto>();


    }
}
