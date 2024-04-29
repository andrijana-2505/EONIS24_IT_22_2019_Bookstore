using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;

namespace BackendBookstore.DTOs.ReadDTO
{
    public class CategoryReadDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public virtual ICollection<BookUpdateDto> Books { get; set; } = new List<BookUpdateDto>();
    }
}
