using MediatR;

namespace backend.Application.Commands
{
    /// <summary>
    /// Command para eliminar un destino
    /// </summary>
    public class DeleteDestinationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
