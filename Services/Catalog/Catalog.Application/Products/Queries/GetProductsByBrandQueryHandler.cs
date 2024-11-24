using Catalog.Application.DTOs.Products;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Queries
{
    public class GetProductsByBrandQuery : IRequest<IEnumerable<ProductDTO>>
    {
        public string BrandName { get; set; }
        public GetProductsByBrandQuery(string brandName)
        {
            BrandName = brandName;
        }
    }
    public class GetProductsByBrandQueryHandler : IRequestHandler<GetProductsByBrandQuery, IEnumerable<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByBrandQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var brandName = request.BrandName;
            var products = await _productRepository.GetByBrand(brandName, cancellationToken);
            var result = CustomMapper.Mapper.Map<IEnumerable<ProductDTO>>(products);
            return result;
        }
    }
}
