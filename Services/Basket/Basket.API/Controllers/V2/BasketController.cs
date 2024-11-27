using Asp.Versioning;
using Basket.Application.Basket.Commands;
using Basket.Application.Basket.Queries;
using Basket.Application.Mappers;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IMediator mediator, ILogger<BasketController> logger, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Checkout Basket Version 2
        /// </summary>
        /// <param name="model">BasketCheckoutV2 object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 model)
        {
            // Get existing basket by username
            var query = new GetBasketByUserNameQuery(model.UserName);
            var basket = await _mediator.Send(query);
            if (basket is null)
                return BadRequest();

            var eventMsg = CustomMapper.Mapper.Map<BasketCheckoutEventV2>(model);
            eventMsg.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMsg);

            _logger.LogInformation($"Basket Published for {basket.UserName} with v2 version");

            // delete basket
            var deleteQuery = new DeleteShoppingCartCommand(model.UserName);
            await _mediator.Send(deleteQuery);

            return Accepted();
        }
    }
}
