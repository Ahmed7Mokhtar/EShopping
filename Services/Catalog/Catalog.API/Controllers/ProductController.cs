using Catalog.Application.DTOs.Products;
using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Queries;
using Catalog.Core.Specifications;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;

namespace Catalog.API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Pagination<ProductDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetAsync([FromQuery] CatalogSpecParams catalogSpecParams)
        {
            var query = new GetAllProductsQuery(catalogSpecParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("[action]/{id}", Name = nameof(GetByIdAsync))]
        [ProducesResponseType(typeof(ProductDTO), (int)HttpStatusCode.OK)]
        //[ProducesErrorResponseType(typeof(NotFoundResult))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetByIdAsync(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);

            _logger.LogInformation($"Product with Id: {result.Id} fetched!");

            return Ok(result);
        }

        [HttpGet("[action]/{name}", Name = nameof(GetByNameAsync))]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), (int)HttpStatusCode.OK)]
        //[ProducesErrorResponseType(typeof(NotFoundResult))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetByNameAsync(string name)
        {
            var query = new GetProductsByNameQuery(name);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("[action]/{name}", Name = nameof(GetByBrandNameAsync))]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), (int)HttpStatusCode.OK)]
        //[ProducesErrorResponseType(typeof(NotFoundResult))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetByBrandNameAsync(string name)
        {
            var query = new GetProductsByBrandQuery(name);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(ProductDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<ProductDTO>> CreateAsync(CreateProductDTO model)
        {
            var query = new CreateProductCommand(model);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateProductDTO model)
        {
            var query = new UpdateProductCommand(model);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        [Route("Delete/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var query = new DeleteProductCommand(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
