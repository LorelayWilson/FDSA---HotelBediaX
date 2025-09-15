using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Adapters
{
    /// <summary>
    /// Mapper específico para conversiones de Destination
    /// Proporciona métodos estáticos para conversiones específicas
    /// </summary>
    public static class DestinationMapper
    {
        /// <summary>
        /// Convierte entidad Destination a DestinationDto
        /// </summary>
        public static DestinationDto ToDto(Destination entity)
        {
            return new DestinationDto
            {
                ID = entity.ID,
                Name = entity.Name,
                Description = entity.Description,
                CountryCode = entity.CountryCode,
                Type = entity.Type,
                LastModif = entity.LastModif
            };
        }

        /// <summary>
        /// Convierte CreateDestinationDto a entidad Destination
        /// </summary>
        public static Destination ToEntity(CreateDestinationDto dto)
        {
            return new Destination
            {
                Name = dto.Name,
                Description = dto.Description,
                CountryCode = dto.CountryCode,
                Type = dto.Type,
                LastModif = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Actualiza entidad existente con datos de UpdateDestinationDto
        /// </summary>
        public static void UpdateEntity(UpdateDestinationDto dto, Destination existingEntity)
        {
            existingEntity.Name = dto.Name;
            existingEntity.Description = dto.Description;
            existingEntity.CountryCode = dto.CountryCode;
            existingEntity.Type = dto.Type;
            existingEntity.LastModif = DateTime.UtcNow;
        }

        /// <summary>
        /// Convierte lista de entidades a lista de DTOs
        /// </summary>
        public static List<DestinationDto> ToDtoList(IEnumerable<Destination> entities)
        {
            return entities.Select(ToDto).ToList();
        }
    }
}
