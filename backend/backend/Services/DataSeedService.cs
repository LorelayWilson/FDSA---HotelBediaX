using backend.Data;
using backend.Models;

namespace backend.Services
{
    /// <summary>
    /// Servicio responsable de poblar la base de datos con datos de ejemplo
    /// Se ejecuta automáticamente al iniciar la aplicación si la base está vacía
    /// </summary>
    public class DataSeedService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos
        /// </summary>
        /// <param name="context">Contexto de Entity Framework para insertar datos</param>
        public DataSeedService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método principal que ejecuta el seed de datos
        /// Solo se ejecuta si no hay destinos en la base de datos
        /// </summary>
        public async Task SeedDataAsync()
        {
            // Verificar si ya existen destinos para evitar duplicación
            if (_context.Destinations.Any())
                return;

            // Lista de destinos turísticos de ejemplo
            // Incluye una variedad de países, tipos y descripciones realistas
            var destinations = new List<Destination>
            {
                // Destinos de playa
                new Destination
                {
                    Name = "Playa del Carmen",
                    Description = "Hermosa playa caribeña con aguas cristalinas y arena blanca. Perfecta para buceo y snorkel.",
                    CountryCode = "MEX", // México
                    Type = DestinationType.Beach,
                    LastModif = DateTime.UtcNow
                },
                
                // Destinos culturales
                new Destination
                {
                    Name = "Santorini",
                    Description = "Isla griega famosa por sus casas blancas y azules, puestas de sol espectaculares y vistas al mar Egeo.",
                    CountryCode = "GRC", // Grecia
                    Type = DestinationType.Cultural,
                    LastModif = DateTime.UtcNow
                },
                new Destination
                {
                    Name = "Kyoto",
                    Description = "Antigua capital de Japón con templos históricos, jardines zen y la famosa geisha de Gion.",
                    CountryCode = "JPN", // Japón
                    Type = DestinationType.Cultural,
                    LastModif = DateTime.UtcNow
                },
                
                // Destinos de aventura
                new Destination
                {
                    Name = "Machu Picchu",
                    Description = "Ciudad inca perdida en las alturas de los Andes peruanos, una de las maravillas del mundo.",
                    CountryCode = "PER", // Perú
                    Type = DestinationType.Adventure,
                    LastModif = DateTime.UtcNow
                },
                
                // Destinos urbanos
                new Destination
                {
                    Name = "París",
                    Description = "La ciudad del amor, con la Torre Eiffel, el Louvre y los Campos Elíseos.",
                    CountryCode = "FRA", // Francia
                    Type = DestinationType.City,
                    LastModif = DateTime.UtcNow
                },
                new Destination
                {
                    Name = "Nueva York",
                    Description = "La ciudad que nunca duerme, con Times Square, Central Park y la Estatua de la Libertad.",
                    CountryCode = "USA", // Estados Unidos
                    Type = DestinationType.City,
                    LastModif = DateTime.UtcNow
                },
                new Destination
                {
                    Name = "Barcelona",
                    Description = "Ciudad catalana con la arquitectura de Gaudí, playas mediterráneas y la Sagrada Familia.",
                    CountryCode = "ESP", // España
                    Type = DestinationType.Cultural,
                    LastModif = DateTime.UtcNow
                },
                new Destination
                {
                    Name = "Río de Janeiro",
                    Description = "Ciudad brasileña famosa por el Cristo Redentor, el Pan de Azúcar y las playas de Copacabana.",
                    CountryCode = "BRA", // Brasil
                    Type = DestinationType.City,
                    LastModif = DateTime.UtcNow
                },
                
                // Destinos de montaña
                new Destination
                {
                    Name = "Alpes Suizos",
                    Description = "Impresionantes montañas para esquí, senderismo y deportes de invierno.",
                    CountryCode = "CHE", // Suiza
                    Type = DestinationType.Mountain,
                    LastModif = DateTime.UtcNow
                },
                
                // Destinos de relajación
                new Destination
                {
                    Name = "Bali",
                    Description = "Isla indonesia famosa por sus templos, playas y ambiente relajante.",
                    CountryCode = "IDN", // Indonesia
                    Type = DestinationType.Relax,
                    LastModif = DateTime.UtcNow
                }
            };

            // Insertar todos los destinos en la base de datos
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();
        }
    }
}
