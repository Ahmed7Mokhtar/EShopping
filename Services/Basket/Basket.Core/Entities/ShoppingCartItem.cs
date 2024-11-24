namespace Basket.Core.Entities
{
    public class ShoppingCartItem
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
    }
}
