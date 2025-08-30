using MediatR;
using backend.DTOs;

namespace backend.Commands
{
    /// <summary>
    /// Command para crear un nuevo destino
    /// </summary>
    public class CreateDestinationCommand : IRequest<DestinationDto>
    {
        public CreateDestinationDto CreateDestinationDto { get; set; } = null!;
    }
}
