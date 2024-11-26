using AutoMapper;
using Discount.Application.Mappers;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Queries
{
    public class GetDiscountQuery : IRequest<CouponModel>
    {
        public string ProductId { get; }
        public GetDiscountQuery(string productId)
        {
            ProductId = productId;
        }
    }

    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDiscountQueryHandler> _logger;

        public GetDiscountQueryHandler(IDiscountRepository discountRepository, IMapper mapper, ILogger<GetDiscountQueryHandler> logger)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _discountRepository.Get(request.ProductId);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for product with Id: {request.ProductId} not found!"));
            }

            var result = new DiscountMapper().Map(coupon);

            _logger.LogInformation($"Discount fetched for product with id: {result.ProductId}, amount: {result.Amount}");

            return result;
        }
    }
}
