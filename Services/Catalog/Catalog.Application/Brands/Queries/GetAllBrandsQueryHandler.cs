using AutoMapper;
using Catalog.Application.DTOs.Brands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Brands.Queries
{
    public class GetAllBrandsQuery : IRequest<IEnumerable<BrandDTO>>
    {
    }
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IEnumerable<BrandDTO>>
    {
        private readonly IBrandRepository _brandRepository;
        //private readonly IMapper _mapper;

        public GetAllBrandsQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            //_mapper = mapper;
        }

        public async Task<IEnumerable<BrandDTO>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAll(cancellationToken);
            //var response = _mapper.Map<IEnumerable<BrandDTO>>(brands);
            var response = CustomMapper.Mapper.Map<IEnumerable<BrandDTO>>(brands);

            return response;
        }
    }
}
