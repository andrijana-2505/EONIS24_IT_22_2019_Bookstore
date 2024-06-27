using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;

namespace BackendBookstore.DTOs.ReadDTO
{
    public class BookReadDto
    {
        public int BookId { get; set; }

        public string BookTitle { get; set; } = null!;

        public string BookAuthor { get; set; } = null!;

        public DateOnly PublishingYear { get; set; }

        public string Publisher { get; set; } = null!;

        public decimal BookPrice { get; set; }

        public int Available { get; set; }

        public int? CategoryId { get; set; }
        public CategoryReadDto Category { get; set; }

        public virtual ICollection<OrderItemUpdateDto> Orderitems { get; set; } = new List<OrderItemUpdateDto>();

        public virtual ICollection<ReviewUpdateDto> Reviews { get; set; } = new List<ReviewUpdateDto>();
    }
}
