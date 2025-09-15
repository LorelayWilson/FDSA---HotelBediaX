using MediatR;
using backend.Domain.Interfaces;

namespace backend.Application.Queries
{
    /// <summary>
    /// Handler para la query de obtener países
    /// </summary>
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<string>>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetCountriesQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<List<string>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _repositoryManager.Destinations.GetUniqueCountryCodesAsync();
        }
    }
}
