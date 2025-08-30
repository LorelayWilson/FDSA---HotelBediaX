using MediatR;
using backend.DTOs;

namespace backend.Commands
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
