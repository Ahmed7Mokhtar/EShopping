using Basket.Application.DTOs;
using Basket.Application.Mappers;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Queries
{
    public class GetBasketByUserBaneQuery : IRequest<ShoppingCartDTO>
    {
        public string UserName { get; set; }
        public GetBasketByUserBaneQuery(string userName)
        {
            UserName = userName;
        }
    }
    public class GetBasketByUserBaneQueryHandler : IRequestHandler<GetBasketByUserBaneQuery, ShoppingCartDTO>
    {
        private readonly IBasketRepository _basketRepository;

        public GetBasketByUserBaneQueryHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<ShoppingCartDTO> Handle(GetBasketByUserBaneQuery request, CancellationToken cancellationToken)
        {
            var cart = await _basketRepository.Get(request.UserName, cancellationToken);
            var result = CustomMapper.Mapper.Map<ShoppingCartDTO>(cart);
            return result;
        }
    }
}
