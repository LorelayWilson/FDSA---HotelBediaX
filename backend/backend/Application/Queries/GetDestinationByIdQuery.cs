using MediatR;
using backend.Application.DTOs;

namespace backend.Application.Queries
{
    /// <summary>
    /// Query para obtener un destino por su ID
    /// </summary>
    public class GetDestinationByIdQuery : IRequest<DestinationDto?>
    {
        public int Id { get; set; }
    }
}
