using MediatR;
using backend.Models;

namespace backend.Queries
{
    /// <summary>
    /// Handler para la query de obtener tipos de destino
    /// </summary>
    public class GetDestinationTypesQueryHandler : IRequestHandler<GetDestinationTypesQuery, List<string>>
    {
        public Task<List<string>> Handle(GetDestinationTypesQuery request, CancellationToken cancellationToken)
        {
            var types = Enum.GetValues<DestinationType>()
                .Select(t => t.ToString())
                .ToList();
            
            return Task.FromResult(types);
        }
    }
}
