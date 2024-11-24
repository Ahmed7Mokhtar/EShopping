using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<ShoppingCart?> Get(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await _cache.GetStringAsync(userName);
            if (string.IsNullOrWhiteSpace(basket))
            {
                return null;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> Update(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await _cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));
            return await Get(cart.UserName);
        }

        public async Task Delete(string userName, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(userName);
        }
    }
}
