namespace Basket.Core.Entities
{
    public class BasketCheckout
    {
        public string UserName { get; set; } = null!;
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        public string? CardName { get; set; }
        public string? CardNum { get; set; }
        public string? CardExpiration { get; set; }
        public string? Cvv { get; set; }
        public int PaymentMethod { get; set; }
    }
}
