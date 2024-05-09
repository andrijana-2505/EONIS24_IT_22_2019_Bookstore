using System;
using System.Collections.Generic;

namespace BackendBookstore.Models;

public partial class Order
{
    public int OrdersId { get; set; }

    public decimal? TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public DateOnly? OrderDate { get; set; }

    public string? StripeTransactionId { get; set; }

    public int? UsersId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual User? Users { get; set; }
}
