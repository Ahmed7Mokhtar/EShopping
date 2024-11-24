using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.DTOs;
using Ordering.Application.Orders.Commands;
using Ordering.Application.Orders.Queries;
using System.Net;

namespace Ordering.API.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("[action]/{userName}", Name = nameof(GetByUserName))]
        [ProducesResponseType(typeof(IEnumerable<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetByUserName(string userName)
        {
            var query = new GetOrdersQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = nameof(Checkout))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout(CheckoutOrderDTO model)
        {
            var command = new CheckoutOrderCommand(model);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = nameof(Update))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(UpdateOrderDTO model)
        {
            var command = new UpdateOrderCommand(model);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("[action]/{id}", Name = nameof(Delete))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
