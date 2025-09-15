using MediatR;
using backend.Application.DTOs;
using backend.Application.Adapters;
using backend.Domain.Interfaces;

namespace backend.Application.Queries
{
    /// <summary>
    /// Handler para la query de obtener destinos con filtros
    /// </summary>
    public class GetDestinationsQueryHandler : IRequestHandler<GetDestinationsQuery, PagedResultDto<DestinationDto>>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetDestinationsQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<PagedResultDto<DestinationDto>> Handle(GetDestinationsQuery request, CancellationToken cancellationToken)
        {
            // Convertir DTO de filtro a criterios del dominio
            var domainFilter = DestinationFilterAdapter.ToDomainCriteria(request.Filter);

            // Obtener destinos con filtros usando el repositorio del dominio
            var domainResult = await _repositoryManager.Destinations.GetDestinationsWithFiltersAsync(domainFilter);

            // Convertir resultado del dominio a DTO usando el adaptador
            return PagedResultAdapter.ToDto(domainResult, entity => DestinationMapper.ToDto(entity));
        }
    }
}
