using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// Enum que define los tipos de destinos turísticos disponibles
    /// </summary>
    public enum DestinationType
    {
        Beach,
        Mountain,
        City,
        Cultural,
        Adventure,
        Relax
    }

    /// <summary>
    /// Modelo principal que representa un destino turístico
    /// </summary>
    public class Destination
    {
        /// <summary>
        /// Identificador único del destino (clave primaria)
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// Nombre del destino turístico
        /// </summary>
        [Required(ErrorMessage = "El nombre del destino es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Descripción detallada del destino
        /// </summary>
        [Required(ErrorMessage = "La descripción del destino es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Código ISO del país (formato de 3 caracteres: MEX, USA, ESP, etc.)
        /// </summary>
        [Required(ErrorMessage = "El código de país es obligatorio")]
        [StringLength(3, ErrorMessage = "El código de país debe tener exactamente 3 caracteres")]
        public string CountryCode { get; set; } = string.Empty;
        
        /// <summary>
        /// Tipo de destino turístico según la categorización
        /// </summary>
        [Required(ErrorMessage = "El tipo de destino es obligatorio")]
        public DestinationType Type { get; set; }
        
        /// <summary>
        /// Fecha y hora de la última modificación del registro
        /// Se actualiza automáticamente en cada operación de escritura
        /// </summary>
        public DateTime LastModif { get; set; } = DateTime.UtcNow;
    }
}
