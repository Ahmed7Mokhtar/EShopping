using Basket.Application.Basket.Commands;
using Basket.Application.Basket.Queries;
using Basket.Application.DTOs;
using Basket.Application.Mappers;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [Route("Get/{userName}")]
        [ProducesResponseType(typeof(ShoppingCartDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartDTO>> GetAsync(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(typeof(ShoppingCartDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartDTO>> CreateOrUpdateAsync(CreateShoppingCartDTO model)
        {
            var command = new CreateShoppingCartCommand(model);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteAsync(string userName)
        {
            var command = new DeleteShoppingCartCommand(userName);
            var result = await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout model)
        {
            // Get existing basket by username
            var query = new GetBasketByUserNameQuery(model.UserName);
            var basket = await _mediator.Send(query);
            if (basket is null)
                return BadRequest();

            var eventMsg = CustomMapper.Mapper.Map<BasketCheckoutEvent>(model);
            eventMsg.TotalPrice = basket.TotalPrice;
            
            await _publishEndpoint.Publish(eventMsg);

            // delete basket
            var deleteQuery = new DeleteShoppingCartCommand(model.UserName);
            await _mediator.Send(deleteQuery);

            return Accepted();
        }
    }
}
