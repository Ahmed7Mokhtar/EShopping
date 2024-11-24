using AutoMapper;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
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

        public GetDiscountQueryHandler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _discountRepository.Get(request.ProductId);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for product with Id: {request.ProductId} not found!"));
            }

            //TODO: Fix mapping
            //var result = _mapper.Map<CouponModel>(coupon);
            var result = new CouponModel
            {
                Id = coupon.Id,
                Amount = (long)coupon.Amount * 100,
                Description = coupon.Description,
                ProductId = coupon.ProductId,
            };

            return result;
        }
    }
}
