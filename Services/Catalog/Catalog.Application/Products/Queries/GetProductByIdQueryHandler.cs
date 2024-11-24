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
    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public string Id { get; set; } = null!;
        public GetProductByIdQuery(string id)
        {
            Id = id;
        }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productId = request.Id;
            var product = await _productRepository.GetById(productId, cancellationToken);
            var result = CustomMapper.Mapper.Map<ProductDTO>(product);
            return result;
        }
    }
}
