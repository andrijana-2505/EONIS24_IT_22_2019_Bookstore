using System;
using System.Collections.Generic;

namespace BackendBookstore.Models;

public partial class Orderitem
{
    public int OrderItemId { get; set; }

    public int? Quantity { get; set; }

    public int? OrdersId { get; set; }

    public int? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Order? Orders { get; set; }
}
