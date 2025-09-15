using backend.Domain.Entities;

namespace backend.Domain.Interfaces
{
    /// <summary>
    /// Interfaz para adaptadores de conversión de destinos
    /// Permite que el dominio defina contratos sin depender de librerías externas
    /// </summary>
    public interface IDestinationAdapter
    {
        /// <summary>
        /// Convierte una entidad Destination a DTO
        /// </summary>
        /// <param name="entity">Entidad del dominio</param>
        /// <returns>DTO de destino</returns>
        object ToDto(Destination entity);

        /// <summary>
        /// Convierte un DTO a entidad Destination
        /// </summary>
        /// <param name="dto">DTO de origen</param>
        /// <returns>Entidad del dominio</returns>
        Destination ToEntity(object dto);

        /// <summary>
        /// Actualiza una entidad existente con datos de un DTO
        /// </summary>
        /// <param name="dto">DTO con datos actualizados</param>
        /// <param name="existingEntity">Entidad existente a actualizar</param>
        void UpdateEntity(object dto, Destination existingEntity);
    }
}
