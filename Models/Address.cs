using System;
using System.Collections.Generic;

namespace BackendBookstore.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public int? OrdersId { get; set; }

    public virtual Order? Orders { get; set; }
}
