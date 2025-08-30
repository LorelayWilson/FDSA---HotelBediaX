using MediatR;
using backend.DTOs;

namespace backend.Queries
{
    /// <summary>
    /// Query para obtener un destino por su ID
    /// </summary>
    public class GetDestinationByIdQuery : IRequest<DestinationDto?>
    {
        public int Id { get; set; }
    }
}
