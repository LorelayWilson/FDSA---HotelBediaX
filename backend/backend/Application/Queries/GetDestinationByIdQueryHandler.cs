using MediatR;
using backend.Application.DTOs;
using backend.Application.Adapters;
using backend.Domain.Interfaces;

namespace backend.Application.Queries
{
    /// <summary>
    /// Handler para la query de obtener destino por ID
    /// </summary>
    public class GetDestinationByIdQueryHandler : IRequestHandler<GetDestinationByIdQuery, DestinationDto?>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetDestinationByIdQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<DestinationDto?> Handle(GetDestinationByIdQuery request, CancellationToken cancellationToken)
        {
            // Buscar el destino por ID
            var destination = await _repositoryManager.Destinations.GetByIdAsync(request.Id);

            // Retornar DTO mapeado o null si no existe usando adaptador
            return destination != null ? DestinationMapper.ToDto(destination) : null;
        }
    }
}
