using MediatR;
using backend.Application.DTOs;
using backend.Application.Adapters;
using backend.Domain.Interfaces;
using Serilog;

namespace backend.Application.Commands
{
    /// <summary>
    /// Handler para el comando de actualizar destino
    /// </summary>
    public class UpdateDestinationCommandHandler : IRequestHandler<UpdateDestinationCommand, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDestinationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DestinationDto?> Handle(UpdateDestinationCommand request, CancellationToken cancellationToken)
        {
            // Buscar el destino a actualizar
            var destination = await _unitOfWork.Destinations.GetByIdAsync(request.Id);

            if (destination == null)
            {
                Log.Warning("Destino no encontrado para actualizar con ID: {DestinationId}", request.Id);
                return null;
            }

            // Aplicar los cambios del DTO a la entidad usando adaptador
            DestinationMapper.UpdateEntity(request.UpdateDestinationDto, destination);

            // Actualizar en la base de datos
            _unitOfWork.Destinations.Update(destination);
            await _unitOfWork.SaveChangesAsync();

            Log.Information("Destino actualizado: {DestinationName} (ID: {DestinationId})", 
                destination.Name, destination.ID);

            // Retornar DTO del destino actualizado usando adaptador
            return DestinationMapper.ToDto(destination);
        }
    }
}
