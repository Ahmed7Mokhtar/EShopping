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
    public class TypeRepository : ITypeRepository
    {
        private readonly ICatalogContext _catalogContext;

        public TypeRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        public async Task<IEnumerable<ProductType>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _catalogContext.Types
                .Find(m => true)
                .ToListAsync(cancellationToken);
        }
    }
}
