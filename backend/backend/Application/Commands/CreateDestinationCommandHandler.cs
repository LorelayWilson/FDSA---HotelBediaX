using MediatR;
using backend.Application.DTOs;
using backend.Application.Adapters;
using backend.Domain.Entities;
using backend.Domain.Interfaces;
using Serilog;

namespace backend.Application.Commands
{
    /// <summary>
    /// Handler para el comando de crear destino
    /// </summary>
    public class CreateDestinationCommandHandler : IRequestHandler<CreateDestinationCommand, DestinationDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDestinationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DestinationDto> Handle(CreateDestinationCommand request, CancellationToken cancellationToken)
        {
            // Mapear DTO a entidad usando adaptador
            var destination = DestinationMapper.ToEntity(request.CreateDestinationDto);

            // Agregar a la base de datos
            _unitOfWork.Destinations.Add(destination);
            await _unitOfWork.SaveChangesAsync();

            Log.Information("Destino creado: {DestinationName} (ID: {DestinationId}) en {CountryCode}", 
                destination.Name, destination.ID, destination.CountryCode);

            // Retornar DTO del destino creado usando adaptador
            return DestinationMapper.ToDto(destination);
        }
    }
}
