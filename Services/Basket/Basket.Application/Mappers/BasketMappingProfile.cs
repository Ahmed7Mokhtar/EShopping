using AutoMapper;
using Basket.Application.DTOs;
using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Mappers
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartItemDTO>().ReverseMap();
            CreateMap<ShoppingCart, CreateShoppingCartDTO>().ReverseMap();
        }
    }
}
