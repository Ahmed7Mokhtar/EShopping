using AutoMapper;
using Catalog.Application.DTOs.Types;
using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Mappers
{
    public class TypeMappingProfile : Profile
    {
        public TypeMappingProfile()
        {
            CreateMap<ProductType, TypeDTO>().ReverseMap();
        }
    }
}
