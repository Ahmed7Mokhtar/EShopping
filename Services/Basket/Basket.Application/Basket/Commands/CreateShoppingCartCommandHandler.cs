using Basket.Application.DTOs;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Basket.Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Commands
{
    public class CreateShoppingCartCommand : BaseCommand<CreateShoppingCartDTO>, IRequest<ShoppingCartDTO>
    {
        public CreateShoppingCartCommand(CreateShoppingCartDTO model)
        {
            Model = model;
        }
    }

    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartDTO>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;

        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
        }

        public async Task<ShoppingCartDTO> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            foreach (var item in model.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductId);
                item.Price -= coupon.Amount / 100M;
            }

            var addedCart = CustomMapper.Mapper.Map<ShoppingCart>(model);

            var cart = await _basketRepository.Update(addedCart, cancellationToken);
            
            var result = CustomMapper.Mapper.Map<ShoppingCartDTO>(cart);
            return result;
        }
    }
}
