using MediatR;
using backend.Application.DTOs;

namespace backend.Application.Queries
{
    /// <summary>
    /// Query para obtener destinos con filtros y paginación
    /// </summary>
    public class GetDestinationsQuery : IRequest<PagedResultDto<DestinationDto>>
    {
        public DestinationFilterDto Filter { get; set; } = null!;
    }
}
