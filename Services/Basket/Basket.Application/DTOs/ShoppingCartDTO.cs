using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.DTOs
{
    public class ShoppingCartDTO
    {
        public string UserName { get; set; } = null!;
        public List<ShoppingCartItemDTO> Items { get; set; } = new List<ShoppingCartItemDTO>();
        public decimal TotalPrice 
        {
            get
            {
                decimal totalPrice = 0;
                totalPrice = Items.Sum(x => x.Price * x.Quantity);
                return totalPrice;
            }
        }

        public ShoppingCartDTO()
        {
            
        }

        public ShoppingCartDTO(string userName)
        {
            UserName = userName;
        }
    }
}
