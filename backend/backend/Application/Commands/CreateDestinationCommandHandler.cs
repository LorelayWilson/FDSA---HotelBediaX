using MediatR;
using AutoMapper;
using backend.Application.DTOs;
using backend.Domain.Entities;
using backend.Infrastructure.UnitOfWork;
using Serilog;

namespace backend.Application.Commands
{
    /// <summary>
    /// Handler para el comando de crear destino
    /// </summary>
    public class CreateDestinationCommandHandler : IRequestHandler<CreateDestinationCommand, DestinationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDestinationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationDto> Handle(CreateDestinationCommand request, CancellationToken cancellationToken)
        {
            // Mapear DTO a entidad
            var destination = _mapper.Map<Destination>(request.CreateDestinationDto);
            
            // Asignar fecha de modificación automáticamente
            destination.LastModif = DateTime.UtcNow;

            // Agregar a la base de datos
            _unitOfWork.Destinations.Add(destination);
            await _unitOfWork.SaveChangesAsync();

            Log.Information("Destino creado: {DestinationName} (ID: {DestinationId}) en {CountryCode}", 
                destination.Name, destination.ID, destination.CountryCode);

            // Retornar DTO del destino creado
            return _mapper.Map<DestinationDto>(destination);
        }
    }
}
