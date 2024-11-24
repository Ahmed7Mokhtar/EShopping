using AutoMapper;
using Catalog.Core.Shared;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Commands
{
    public class CreateDiscountCommand : BaseCommand<CreateDiscountRequest>, IRequest<CouponModel>
    {
        public CreateDiscountCommand(CreateDiscountRequest model)
        {
            Model = model;
        }
    }

    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public CreateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Coupon>(request.Model);
            await _discountRepository.Create(coupon);

            var result = _mapper.Map<CouponModel>(coupon);
            return result;
        }
    }
}
