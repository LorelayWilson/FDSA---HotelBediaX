using backend.Models;
using backend.DTOs;

namespace backend.Repositories
{
    /// <summary>
    /// Interfaz específica para el repositorio de destinos
    /// Extiende la funcionalidad básica del repositorio genérico
    /// </summary>
    public interface IDestinationRepository : IRepository<Destination>
    {
        /// <summary>
        /// Obtiene destinos con filtros y paginación
        /// </summary>
        /// <param name="filter">Filtros de búsqueda y paginación</param>
        /// <returns>Resultado paginado de destinos</returns>
        Task<PagedResultDto<Destination>> GetDestinationsWithFiltersAsync(DestinationFilterDto filter);

        /// <summary>
        /// Obtiene la lista de códigos de países únicos
        /// </summary>
        /// <returns>Lista de códigos de países ordenados</returns>
        Task<List<string>> GetUniqueCountryCodesAsync();

        /// <summary>
        /// Obtiene destinos por tipo específico
        /// </summary>
        /// <param name="type">Tipo de destino</param>
        /// <returns>Lista de destinos del tipo especificado</returns>
        Task<List<Destination>> GetDestinationsByTypeAsync(DestinationType type);

        /// <summary>
        /// Busca destinos por término de búsqueda
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de destinos que coinciden con el término</returns>
        Task<List<Destination>> SearchDestinationsAsync(string searchTerm);
    }
}
