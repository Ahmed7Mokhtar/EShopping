﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.DTOs
{
    public class CheckoutOrderDTO
    {
        public string UserName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Country { get; set; }
        public string? AddressLine { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string? CardName { get; set; }
        public string? CardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? Cvv { get; set; }
        public int? PaymentMethod { get; set; }
    }
}