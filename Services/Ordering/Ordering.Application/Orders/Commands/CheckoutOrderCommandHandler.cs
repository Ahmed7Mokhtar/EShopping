using AutoMapper;
using Ordering.Core.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.DTOs;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands
{
    public class CheckoutOrderCommand : BaseCommand<CheckoutOrderDTO>, IRequest<string>
    {
        public CheckoutOrderCommand(CheckoutOrderDTO model)
        {
            Model = model;
        }
    }

    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.Model);
            
            var addedOrder = await _orderRepository.AddAsync(order, cancellationToken);
            
            _logger.LogInformation($"Order with id {addedOrder.Id} created successfully!");

            return addedOrder.Id;
        }
    }
}
