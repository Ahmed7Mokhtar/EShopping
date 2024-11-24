using Catalog.Application.DTOs.Products;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Queries
{
    public class GetAllProductsQuery : IRequest<Pagination<ProductDTO>>
    {
        public CatalogSpecParams SpecParams { get; set; }
        public GetAllProductsQuery(CatalogSpecParams catalogSpecParams)
        {
            SpecParams = catalogSpecParams ??= new CatalogSpecParams();
        }
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Pagination<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAll(request.SpecParams, cancellationToken);
            var result = CustomMapper.Mapper.Map<Pagination<ProductDTO>>(products);
            return result;
        }
    }
}
