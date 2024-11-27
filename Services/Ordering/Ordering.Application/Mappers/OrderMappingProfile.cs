using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.DTOs;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, CheckoutOrderDTO>().ReverseMap();
            CreateMap<Order, CheckoutOrderV2DTO>().ReverseMap();
            CreateMap<Order, UpdateOrderDTO>().ReverseMap();
            CreateMap<CheckoutOrderDTO, BasketCheckoutEvent>().ReverseMap();
            CreateMap<CheckoutOrderV2DTO, BasketCheckoutEventV2>().ReverseMap();
        }
    }
}
