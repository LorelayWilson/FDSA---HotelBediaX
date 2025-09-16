using MediatR;
using backend.Domain.Interfaces;
using Serilog;

namespace backend.Application.Commands
{
    /// <summary>
    /// Handler para el comando de eliminar destino
    /// </summary>
    public class DeleteDestinationCommandHandler : IRequestHandler<DeleteDestinationCommand, bool>
    {
        private readonly IRepositoryManager _repositoryManager;

        public DeleteDestinationCommandHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<bool> Handle(DeleteDestinationCommand request, CancellationToken cancellationToken)
        {
            // Buscar el destino a eliminar
            var destination = await _repositoryManager.Destinations.GetByIdAsync(request.Id);

            if (destination is null)
            {
                Log.Warning("Destino no encontrado para eliminar con ID: {DestinationId}", request.Id);
                return false;
            }

            // Eliminar de la base de datos
                _repositoryManager.Destinations.Remove(destination);
                await _repositoryManager.SaveChangesAsync();

            Log.Information("Destino eliminado: {DestinationName} (ID: {DestinationId})", 
                destination.Name, destination.ID);

            return true;
        }
    }
}
