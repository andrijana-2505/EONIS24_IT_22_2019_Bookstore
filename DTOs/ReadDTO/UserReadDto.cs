using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;

namespace BackendBookstore.DTOs.ReadDTO
{
    public class UserReadDto
    {
        public int UsersId { get; set; }

        public string? UserRole { get; set; }

        public string Username { get; set; } = null!;

        //public string PasswordLogin { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public char? Genre { get; set; }

        public virtual ICollection<OrderUpdateDto> Orders { get; set; } = new List<OrderUpdateDto>();

        public virtual ICollection<ReviewUpdateDto> Reviews { get; set; } = new List<ReviewUpdateDto>();
    }
}
