using Basket.Application.Basket.Commands;
using Basket.Application.Basket.Queries;
using Basket.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IMediator _mediator;
        
        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Get/{userName}")]
        [ProducesResponseType(typeof(ShoppingCartDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartDTO>> GetAsync(string userName)
        {
            var query = new GetBasketByUserBaneQuery(userName);
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
    }
}
