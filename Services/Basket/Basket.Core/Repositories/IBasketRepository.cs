using Basket.Core.Entities;

namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> Get(string userName, CancellationToken cancellationToken = default);
        Task<ShoppingCart> Update(ShoppingCart cart, CancellationToken cancellationToken = default);
        Task Delete(string userName, CancellationToken cancellationToken = default);
    }
}
