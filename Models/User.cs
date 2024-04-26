using System;
using System.Collections.Generic;

namespace BackendBookstore.Models;

public partial class User
{
    public int UsersId { get; set; }

    public string? UserRole { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordLogin { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public char? Genre { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
