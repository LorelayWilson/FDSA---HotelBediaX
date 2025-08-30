using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs
{
    /// <summary>
    /// DTO para transferir información completa de un destino
    /// Se usa para respuestas de la API
    /// </summary>
    public class DestinationDto
    {
        /// <summary>Identificador único del destino</summary>
        public int ID { get; set; }
        /// <summary>Nombre del destino turístico</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Descripción detallada del destino</summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>Código ISO del país (3 caracteres)</summary>
        public string CountryCode { get; set; } = string.Empty;
        /// <summary>Tipo de destino turístico</summary>
        public DestinationType Type { get; set; }
        /// <summary>Fecha de última modificación</summary>
        public DateTime LastModif { get; set; }
    }

    /// <summary>
    /// DTO para crear un nuevo destino
    /// No incluye ID ni LastModif ya que se generan automáticamente
    /// </summary>
    public class CreateDestinationDto
    {
        /// <summary>Nombre del destino turístico</summary>
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>Descripción detallada del destino</summary>
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La descripción debe tener entre 1 y 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        /// <summary>Código ISO del país (3 caracteres)</summary>
        [Required(ErrorMessage = "El código de país es requerido")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El código de país debe tener exactamente 3 caracteres")]
        public string CountryCode { get; set; } = string.Empty;
        
        /// <summary>Tipo de destino turístico</summary>
        [Required(ErrorMessage = "El tipo de destino es requerido")]
        public DestinationType Type { get; set; }
    }

    /// <summary>
    /// DTO para actualizar un destino existente
    /// No incluye ID ni LastModif ya que se manejan internamente
    /// </summary>
    public class UpdateDestinationDto
    {
        /// <summary>Nombre del destino turístico</summary>
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>Descripción detallada del destino</summary>
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La descripción debe tener entre 1 y 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        /// <summary>Código ISO del país (3 caracteres)</summary>
        [Required(ErrorMessage = "El código de país es requerido")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El código de país debe tener exactamente 3 caracteres")]
        public string CountryCode { get; set; } = string.Empty;
        
        /// <summary>Tipo de destino turístico</summary>
        [Required(ErrorMessage = "El tipo de destino es requerido")]
        public DestinationType Type { get; set; }
    }

    /// <summary>
    /// DTO para filtrar y paginar la lista de destinos
    /// Todos los campos son opcionales para permitir filtros flexibles
    /// </summary>
    public class DestinationFilterDto
    {
        /// <summary>Término de búsqueda que se aplica a nombre, descripción y código de país</summary>
        public string? SearchTerm { get; set; }
        /// <summary>Filtro por código de país específico</summary>
        public string? CountryCode { get; set; }
        /// <summary>Filtro por tipo de destino específico</summary>
        public DestinationType? Type { get; set; }
        /// <summary>Número de página actual (comienza en 1)</summary>
        public int Page { get; set; } = 1;
        /// <summary>Número de elementos por página (máximo 100 recomendado)</summary>
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// DTO genérico para resultados paginados
    /// Se puede usar con cualquier tipo de entidad
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que se está paginando</typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>Lista de elementos de la página actual</summary>
        public List<T> Items { get; set; } = new();
        /// <summary>Número total de elementos que coinciden con los filtros</summary>
        public int TotalCount { get; set; }
        /// <summary>Número de página actual</summary>
        public int Page { get; set; }
        /// <summary>Número de elementos por página</summary>
        public int PageSize { get; set; }
        /// <summary>Número total de páginas calculado automáticamente</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
