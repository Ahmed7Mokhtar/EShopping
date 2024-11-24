using Catalog.Core.Entities;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(CatalogSpecParams catalogSpecParams)
        {
            if (!string.IsNullOrWhiteSpace(catalogSpecParams.Search))
            {
                AddCriteria(Builders<Product>.Filter.Regex(nameof(Product.Name), new BsonRegularExpression(catalogSpecParams.Search, "i")));
            }

            if (!string.IsNullOrWhiteSpace(catalogSpecParams.BrandId))
            {
                AddCriteria(Builders<Product>.Filter.Eq(p => p.Brand.Id, catalogSpecParams.BrandId));
            }

            if (!string.IsNullOrWhiteSpace(catalogSpecParams.TypeId))
            {
                AddCriteria(Builders<Product>.Filter.Eq(p => p.Type.Id, catalogSpecParams.TypeId));
            }

            ApplyPaging((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize, catalogSpecParams.PageSize);

            if (!string.IsNullOrWhiteSpace(catalogSpecParams.Sort))
            {
                switch (catalogSpecParams.Sort)
                {
                    case "priceAsc":
                        ApplySorting(Builders<Product>.Sort.Ascending(p => p.Price));
                        break;
                    case "priceDesc":
                        ApplySorting(Builders<Product>.Sort.Descending(p => p.Price));
                        break;
                    default:
                        ApplySorting(Builders<Product>.Sort.Ascending(p => p.Name)); // Default
                        break;
                }
            }
        }
    }
}
