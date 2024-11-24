using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.DTOs
{
    public class CreateShoppingCartDTO
    {
        public string UserName { get; set; } = null!;
        public List<ShoppingCartItemDTO> Items { get; set; } = null!;
    }
}
