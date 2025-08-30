using MediatR;
using backend.Application.DTOs;

namespace backend.Application.Commands
{
    /// <summary>
    /// Command para crear un nuevo destino
    /// </summary>
    public class CreateDestinationCommand : IRequest<DestinationDto>
    {
        public CreateDestinationDto CreateDestinationDto { get; set; } = null!;
    }
}
