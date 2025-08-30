using MediatR;
using AutoMapper;
using backend.Application.DTOs;
using backend.Infrastructure.UnitOfWork;
using Serilog;

namespace backend.Application.Commands
{
    /// <summary>
    /// Handler para el comando de actualizar destino
    /// </summary>
    public class UpdateDestinationCommandHandler : IRequestHandler<UpdateDestinationCommand, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDestinationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            // Aplicar los cambios del DTO a la entidad
            _mapper.Map(request.UpdateDestinationDto, destination);
            
            // Actualizar fecha de modificación
            destination.LastModif = DateTime.UtcNow;

            // Actualizar en la base de datos
            _unitOfWork.Destinations.Update(destination);
            await _unitOfWork.SaveChangesAsync();

            Log.Information("Destino actualizado: {DestinationName} (ID: {DestinationId})", 
                destination.Name, destination.ID);

            // Retornar DTO del destino actualizado
            return _mapper.Map<DestinationDto>(destination);
        }
    }
}
