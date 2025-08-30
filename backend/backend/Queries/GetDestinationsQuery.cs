using MediatR;
using backend.DTOs;

namespace backend.Queries
{
    /// <summary>
    /// Query para obtener destinos con filtros y paginación
    /// </summary>
    public class GetDestinationsQuery : IRequest<PagedResultDto<DestinationDto>>
    {
        public DestinationFilterDto Filter { get; set; } = null!;
    }
}
