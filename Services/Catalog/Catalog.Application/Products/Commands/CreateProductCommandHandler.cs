using Catalog.Application.DTOs.Products;
using Catalog.Application.Mappers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Commands
{
    public class CreateProductCommand : BaseCommand<CreateProductDTO>, IRequest<ProductDTO>
    {
        public CreateProductCommand(CreateProductDTO model)
        {
            Model = model;
        }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDTO>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productToAdd = CustomMapper.Mapper.Map<Product>(request.Model);
            if (productToAdd is null)
            {
                throw new ArgumentException("Issue with creating new product!");
            }

            var addedProduct = await _productRepository.Create(productToAdd, cancellationToken);
            var result = CustomMapper.Mapper.Map<ProductDTO>(addedProduct);
            return result;
        }
    }
}
