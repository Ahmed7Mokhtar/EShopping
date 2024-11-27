using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.DTOs;
using Ordering.Application.Orders.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;

        public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _logger.BeginScope("Consuming basket v2 checkout event for {correlationId}", context.Message.CorrelationId);

            var command = new CheckoutOrderV2Command(_mapper.Map<CheckoutOrderV2DTO>(context.Message));
            var result = await _mediator.Send(command);


            _logger.LogInformation("Basket checkout v2 event completed!");
        }
    }
}
