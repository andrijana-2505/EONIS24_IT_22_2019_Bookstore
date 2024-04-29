using BackendBookstore.DTOs.ReadDTO;

namespace BackendBookstore.DTOs
{
    public class BookResponseDto
    {
        public IEnumerable<BookReadDto>? Books { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }

    }
}
