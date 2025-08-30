using MediatR;
using AutoMapper;
using backend.Application.DTOs;
using backend.Infrastructure.UnitOfWork;

namespace backend.Application.Queries
{
    /// <summary>
    /// Handler para la query de obtener destino por ID
    /// </summary>
    public class GetDestinationByIdQueryHandler : IRequestHandler<GetDestinationByIdQuery, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDestinationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationDto?> Handle(GetDestinationByIdQuery request, CancellationToken cancellationToken)
        {
            // Buscar el destino por ID
            var destination = await _unitOfWork.Destinations.GetByIdAsync(request.Id);

            // Retornar DTO mapeado o null si no existe
            return destination != null ? _mapper.Map<DestinationDto>(destination) : null;
        }
    }
}
