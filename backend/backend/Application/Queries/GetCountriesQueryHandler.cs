using MediatR;
using backend.Domain.Interfaces;

namespace backend.Application.Queries
{
    /// <summary>
    /// Handler para la query de obtener países
    /// </summary>
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCountriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<string>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Destinations.GetUniqueCountryCodesAsync();
        }
    }
}
