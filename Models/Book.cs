using System;
using System.Collections.Generic;

namespace BackendBookstore.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string BookAuthor { get; set; } = null!;

    public DateOnly PublishingYear { get; set; }

    public string Publisher { get; set; } = null!;

    public decimal BookPrice { get; set; }

    public int Available { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
