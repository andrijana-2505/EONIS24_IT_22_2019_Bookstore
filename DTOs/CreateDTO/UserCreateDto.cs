namespace BackendBookstore.DTOs.CreateDTO
{
    public class UserCreateDto
    {
        public string Username { get; set; } = null!;

        public string PasswordLogin { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public char? Genre { get; set; }

    }
}
