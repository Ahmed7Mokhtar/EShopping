﻿using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.DTOs;
using Ordering.Application.Orders.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumer> _logger;

        public BasketOrderingConsumer(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _logger.BeginScope("Consuming basket checkout event for {correlationId}", context.Message.CorrelationId);

            var command = new CheckoutOrderCommand(_mapper.Map<CheckoutOrderDTO>(context.Message));
            var result = await _mediator.Send(command);


            _logger.LogInformation("Basket checkout event completed!");
        }
    }
}
