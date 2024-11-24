namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = null!;
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {
            
        }
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
    }
}
