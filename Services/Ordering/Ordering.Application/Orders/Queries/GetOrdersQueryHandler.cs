using AutoMapper;
using MediatR;
using Ordering.Application.DTOs;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries
{
    public class GetOrdersQuery : IRequest<IEnumerable<OrderDTO>>
    {
        public string UserName { get; }
        public GetOrdersQuery(string userName)
        {
            UserName = userName;
        }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDTO>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetByUserName(request.UserName, cancellationToken);
            var result = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            return result;
        }
    }
}
