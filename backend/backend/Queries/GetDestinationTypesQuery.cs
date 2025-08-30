using MediatR;

namespace backend.Queries
{
    /// <summary>
    /// Query para obtener la lista de tipos de destino disponibles
    /// </summary>
    public class GetDestinationTypesQuery : IRequest<List<string>>
    {
    }
}
