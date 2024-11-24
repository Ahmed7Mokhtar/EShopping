using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ICatalogContext _catalogContext;

        public BrandRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        public async Task<IEnumerable<ProductBrand>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _catalogContext.Brands
                .Find(m => true)
                .ToListAsync(cancellationToken);
        }
    }
}
