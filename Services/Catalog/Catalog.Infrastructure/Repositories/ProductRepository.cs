using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Evaluators;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<Pagination<Product>> GetAll(CatalogSpecParams catalogSpecParams, CancellationToken cancellationToken = default)
        {
            var spec = new ProductSpecification(catalogSpecParams);
            IReadOnlyList<Product> datas = await SpecificationEvaluator<Product>.GetQuery(_catalogContext.Products, spec, cancellationToken);
            var totalCount = await _catalogContext.Products.CountDocumentsAsync(spec.Criteria);
            return new Pagination<Product>(catalogSpecParams.PageIndex, catalogSpecParams.PageSize, totalCount, datas);



            //var filter = BuildFilter(catalogSpecParams);
            //var totalItems = await _catalogContext.Products.CountDocumentsAsync(filter);
            //IReadOnlyList<Product> data = await DataFilter(catalogSpecParams, filter, cancellationToken);
            //return new Pagination<Product>(catalogSpecParams.PageIndex, catalogSpecParams.PageSize, totalItems, data);
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter, CancellationToken cancellationToken)
        {
            var sss = _catalogContext.Products;

            //return await _catalogContext.Products
            //                .Find(filter)
            //                .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
            //                .Limit(catalogSpecParams.PageSize)
            //                .ToListAsync(cancellationToken);

            var sordDef = Builders<Product>.Sort.Ascending("Name"); // Default
            if(!string.IsNullOrWhiteSpace(catalogSpecParams.Sort))
            {
                switch (catalogSpecParams.Sort)
                {
                    case "priceAsc":
                        sordDef = Builders<Product>.Sort.Ascending(m => m.Price); 
                        break;
                    case "priceDesc":
                        sordDef = Builders<Product>.Sort.Descending(m => m.Price);
                        break;
                    default:
                        sordDef = Builders<Product>.Sort.Ascending(m => m.Name); // Default
                        break;
                }
            }

            return await _catalogContext.Products
                .Find(filter)
                .Sort(sordDef)
                .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> GetById(string id, CancellationToken cancellationToken = default)
        {
            return await _catalogContext.Products
                .Find(m => m.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<Product>> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _catalogContext.Products
                .Find(m => m.Name.ToLower() == name.ToLower())
                .ToListAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<Product>> GetByBrand(string brandName, CancellationToken cancellationToken = default)
        {
            return await _catalogContext.Products
                .Find(m => m.Brand.Name.ToLower() == brandName.ToLower())
                .ToListAsync(cancellationToken);
        }
        
        public async Task<Product> Create(Product product, CancellationToken cancellationToken = default)
        {
            await _catalogContext.Products.InsertOneAsync(product);
            return product;
        }
        
        public async Task<bool> Update(Product product, CancellationToken cancellationToken = default)
        {
            var updatedProduct = await _catalogContext.Products
                .ReplaceOneAsync(m => m.Id == product.Id, product);
            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            var deleted = await _catalogContext.Products
                .DeleteOneAsync(m => m.Id == id);
            return deleted.IsAcknowledged && deleted.DeletedCount > 0;
        }

        private FilterDefinition<Product> BuildFilter(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrWhiteSpace(catalogSpecParams.Search))
            {
                filter = filter & builder.Regex(m => m.Name, new BsonRegularExpression(catalogSpecParams.Search, "i")); // Case-insensitive search
            }
            if (!string.IsNullOrWhiteSpace(catalogSpecParams.BrandId))
            {
                filter = filter & builder.Eq(m => m.Brand.Id, catalogSpecParams.BrandId);
            }
            if (!string.IsNullOrWhiteSpace(catalogSpecParams.TypeId))
            {
                filter = filter & builder.Eq(m => m.Type.Id, catalogSpecParams.TypeId);
            }

            return filter;
        }
    }
}
