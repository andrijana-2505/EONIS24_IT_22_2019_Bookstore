﻿using BackendBookstore.Models;

namespace BackendBookstore.DTOs.UpdateDTO
{
    public class OrderUpdateDto
    {
        public int OrdersId { get; set; }

        public decimal? TotalAmount { get; set; }

        public OrderStatus? Status { get; set; }

        public DateOnly? OrderDate { get; set; }

        public string? StripeTransactionId { get; set; }

        public int? UsersId { get; set; }
    }
}
