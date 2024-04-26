using System;
using System.Collections.Generic;

namespace BackendBookstore.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int Rating { get; set; }

    public DateOnly? ReviewDate { get; set; }

    public int? UsersId { get; set; }

    public int? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? Users { get; set; }
}
