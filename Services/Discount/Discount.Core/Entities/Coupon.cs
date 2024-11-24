namespace Discount.Core.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductId { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
