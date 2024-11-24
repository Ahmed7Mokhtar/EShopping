using Catalog.Infrastructure.Data.SeedDataContexts;
using Catalog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure
{
    public class CatalogSeeder
    {
        private readonly ICatalogContext _catalogContext;

        public CatalogSeeder(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task SeedAsync()
        {
            await BrandContextSeed.SeedData(_catalogContext.Brands);
            await TypeContextSeed.SeedData(_catalogContext.Types);
            await ProductContextSeed.SeedData(_catalogContext.Products);
        }
    }
}
