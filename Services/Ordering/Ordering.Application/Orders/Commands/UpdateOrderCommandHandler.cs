using AutoMapper;
using Catalog.Core.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.DTOs;
using Ordering.Core.Entities;
using Ordering.Core.Exceptions;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands
{
    public class UpdateOrderCommand : BaseCommand<UpdateOrderDTO>, IRequest<Unit>
    {
        public UpdateOrderCommand(UpdateOrderDTO model)
        {
            Model = model;
        }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model!;
            var order = await _orderRepository.GetByIdAsync(model.Id);
            if (order is null)
            {
                throw new OrderNotFoundException($"Order with id: {model.Id} doesn't exist!");
            }

            _mapper.Map(model, order, typeof(UpdateOrderDTO), typeof(Order));

            await _orderRepository.UpdateAsync(order, cancellationToken);

            _logger.LogInformation($"order with id {order.Id} updated successfully!");

            return Unit.Value;
        }
    }
}
