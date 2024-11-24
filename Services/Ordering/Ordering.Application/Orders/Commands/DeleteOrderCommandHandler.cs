using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Exceptions;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public string Id { get; }
        public DeleteOrderCommand(string Id)
        {
            this.Id = Id;
        }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (order is null)
            {
                throw new OrderNotFoundException($"Order with id: {request.Id} doesn't exist!");
            }

            await _orderRepository.DeleteAsync(order, cancellationToken);

            _logger.LogInformation($"order with id {request.Id} deleted successfully!");

            return Unit.Value;
        }
    }
}
