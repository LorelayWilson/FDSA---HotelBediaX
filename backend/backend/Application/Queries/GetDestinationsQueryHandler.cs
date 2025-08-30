using MediatR;
using AutoMapper;
using backend.Application.DTOs;
using backend.Infrastructure.UnitOfWork;

namespace backend.Application.Queries
{
    /// <summary>
    /// Handler para la query de obtener destinos con filtros
    /// </summary>
    public class GetDestinationsQueryHandler : IRequestHandler<GetDestinationsQuery, PagedResultDto<DestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDestinationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<DestinationDto>> Handle(GetDestinationsQuery request, CancellationToken cancellationToken)
        {
            // Obtener destinos con filtros usando el repositorio
            var result = await _unitOfWork.Destinations.GetDestinationsWithFiltersAsync(request.Filter);

            // Mapear entidades a DTOs
            var destinationDtos = _mapper.Map<List<DestinationDto>>(result.Items);

            return new PagedResultDto<DestinationDto>
            {
                Items = destinationDtos,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
    }
}
