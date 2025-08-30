using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    /// <summary>
    /// Interfaz que define las operaciones disponibles para el servicio de destinos
    /// Implementa el patrón Repository para la gestión de entidades Destination
    /// </summary>
    public interface IDestinationService
    {
        /// <summary>
        /// Obtiene una lista paginada de destinos con filtros opcionales
        /// </summary>
        /// <param name="filter">Criterios de filtrado y paginación</param>
        /// <returns>Resultado paginado con destinos y metadatos de paginación</returns>
        Task<PagedResultDto<DestinationDto>> GetDestinationsAsync(DestinationFilterDto filter);

        /// <summary>
        /// Obtiene un destino específico por su identificador único
        /// </summary>
        /// <param name="id">Identificador del destino a buscar</param>
        /// <returns>DTO del destino si existe, null en caso contrario</returns>
        Task<DestinationDto?> GetDestinationByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo destino en el sistema
        /// </summary>
        /// <param name="createDto">Datos del destino a crear</param>
        /// <returns>DTO del destino creado con ID y fecha de modificación asignados</returns>
        Task<DestinationDto> CreateDestinationAsync(CreateDestinationDto createDto);

        /// <summary>
        /// Actualiza un destino existente en el sistema
        /// </summary>
        /// <param name="id">Identificador del destino a actualizar</param>
        /// <param name="updateDto">Nuevos datos del destino</param>
        /// <returns>DTO del destino actualizado, null si no existe</returns>
        Task<DestinationDto?> UpdateDestinationAsync(int id, UpdateDestinationDto updateDto);

        /// <summary>
        /// Elimina permanentemente un destino del sistema
        /// </summary>
        /// <param name="id">Identificador del destino a eliminar</param>
        /// <returns>True si se eliminó correctamente, False si no existe</returns>
        Task<bool> DeleteDestinationAsync(int id);

        /// <summary>
        /// Obtiene la lista de códigos de países únicos disponibles
        /// Útil para llenar dropdowns y filtros en la interfaz de usuario
        /// </summary>
        /// <returns>Lista ordenada de códigos de países únicos</returns>
        Task<List<string>> GetCountriesAsync();
    }
}
