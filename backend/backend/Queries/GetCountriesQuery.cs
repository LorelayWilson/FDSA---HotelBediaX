using MediatR;

namespace backend.Queries
{
    /// <summary>
    /// Query para obtener la lista de países disponibles
    /// </summary>
    public class GetCountriesQuery : IRequest<List<string>>
    {
    }
}
