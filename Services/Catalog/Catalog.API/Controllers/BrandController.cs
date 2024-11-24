using Catalog.Application.Brands.Queries;
using Catalog.Application.DTOs.Brands;
using Catalog.Application.DTOs.Products;
using Catalog.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;

namespace Catalog.API.Controllers
{
    public class BrandController : BaseController
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BrandDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAsync()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
