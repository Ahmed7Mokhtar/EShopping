using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Pagination<Product>> GetAll(CatalogSpecParams catalogSpecParams, CancellationToken cancellationToken = default);
        Task<Product> GetById(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetByName(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetByBrand(string brandName, CancellationToken cancellationToken = default);
        Task<Product> Create(Product product, CancellationToken cancellationToken = default);
        Task<bool> Update(Product product, CancellationToken cancellationToken = default);
        Task<bool> Delete(string id, CancellationToken cancellationToken = default);
    }
}
