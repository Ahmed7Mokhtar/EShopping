using Catalog.Application.DTOs.Types;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Types.Queries
{
    public class GetAllTypesQuery : IRequest<IEnumerable<TypeDTO>>
    {
    }
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IEnumerable<TypeDTO>>
    {
        private readonly ITypeRepository _typeRepository;

        public GetAllTypesQueryHandler(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }
        public async Task<IEnumerable<TypeDTO>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeRepository.GetAll(cancellationToken);
            var result = CustomMapper.Mapper.Map<IEnumerable<TypeDTO>>(types);
            return result;
        }
    }

}
