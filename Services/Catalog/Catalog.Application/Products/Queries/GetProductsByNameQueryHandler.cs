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
    public class GetProductsByNameQuery : IRequest<IEnumerable<ProductDTO>>
    {
        public string Name { get; set; } = null!;
        public GetProductsByNameQuery(string name)
        {
            Name = name;
        }

    }
    public class GetProductsByNameQueryHandler : IRequestHandler<GetProductsByNameQuery, IEnumerable<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByNameQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            var productName = request.Name;
            var products = await _productRepository.GetByName(productName, cancellationToken);
            var result = CustomMapper.Mapper.Map<IEnumerable<ProductDTO>>(products);
            return result;
        }
    }
}
