using MediatR;

namespace backend.Commands
{
    /// <summary>
    /// Command para eliminar un destino
    /// </summary>
    public class DeleteDestinationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
