using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public string Id { get; set; } = null!;
        public DeleteProductCommand(string id)
        {
            Id = id;
        }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productId = request.Id;
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentNullException(nameof(productId), "Issue with deleting product!");
            }

            var exists = await _productRepository.GetById(productId, cancellationToken);
            if (exists is null)
            {
                throw new ApplicationException("Product doesn't exist!");
            }

            return await _productRepository.Delete(productId, cancellationToken);
        }
    }
}
