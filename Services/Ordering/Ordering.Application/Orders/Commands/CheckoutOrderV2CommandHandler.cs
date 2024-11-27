using MediatR;
using Ordering.Core.Shared;
using Ordering.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ordering.Core.Repositories;
using Ordering.Core.Entities;

namespace Ordering.Application.Orders.Commands
{
    public class CheckoutOrderV2Command : BaseCommand<CheckoutOrderV2DTO>, IRequest<string>
    {
        public CheckoutOrderV2Command(CheckoutOrderV2DTO model)
        {
            Model = model;
        }
    }
    public class CheckoutOrderV2CommandHandler : IRequestHandler<CheckoutOrderV2Command, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderV2CommandHandler> _logger;

        public CheckoutOrderV2CommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderV2CommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> Handle(CheckoutOrderV2Command request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.Model);

            var addedOrder = await _orderRepository.AddAsync(order, cancellationToken);

            _logger.LogInformation($"Order with id {addedOrder.Id} created successfully! using V2");

            return addedOrder.Id;
        }
    }
}
