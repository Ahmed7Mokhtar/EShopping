using Catalog.Application.Brands.Queries;
using Catalog.Application.DTOs.Brands;
using Catalog.Application.DTOs.Types;
using Catalog.Application.Types.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    public class TypeController : BaseController
    {
        private readonly IMediator _mediator;

        public TypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TypeDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAsync()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
