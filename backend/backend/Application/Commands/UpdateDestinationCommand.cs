using MediatR;
using backend.Application.DTOs;

namespace backend.Application.Commands
{
    /// <summary>
    /// Command para actualizar un destino existente
    /// </summary>
    public class UpdateDestinationCommand : IRequest<DestinationDto?>
    {
        public int Id { get; set; }
        public UpdateDestinationDto UpdateDestinationDto { get; set; } = null!;
    }
}
