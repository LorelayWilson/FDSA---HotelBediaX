using backend.Domain.Enums;

namespace backend.Domain.Entities
{
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
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Descripción detallada del destino
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Código ISO del país (formato de 3 caracteres: MEX, USA, ESP, etc.)
        /// </summary>
        public string CountryCode { get; set; } = string.Empty;
        
        /// <summary>
        /// Tipo de destino turístico según la categorización
        /// </summary>
        public DestinationType Type { get; set; }
        
        /// <summary>
        /// Fecha y hora de la última modificación del registro
        /// Se actualiza automáticamente en cada operación de escritura
        /// </summary>
        public DateTime LastModif { get; set; }
    }
}
