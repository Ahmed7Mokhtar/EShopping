using AutoMapper;
using Catalog.Application.DTOs.Brands;
using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Mappers
{
    public class BrandMappingProfile : Profile
    {
        public BrandMappingProfile()
        {
            CreateMap<ProductBrand, BrandDTO>().ReverseMap();
        }
    }
}
