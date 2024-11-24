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
    public class UpdateProductCommand : BaseCommand<UpdateProductDTO>, IRequest<bool>
    {
        public UpdateProductCommand(UpdateProductDTO model)
        {
            Model = model;
        }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productToBeUpdated = CustomMapper.Mapper.Map<Product>(request.Model);
            if (string.IsNullOrWhiteSpace(productToBeUpdated.Id))
            {
                throw new ArgumentException("Issue in updating product!");
            }

            return await _productRepository.Update(productToBeUpdated, cancellationToken);
        }
    }
}
