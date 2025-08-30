using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    /// <summary>
    /// Contexto principal de Entity Framework para la aplicación HotelBediaX
    /// Configurado para usar base de datos en memoria para desarrollo y demos
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor que recibe las opciones de configuración del contexto
        /// </summary>
        /// <param name="options">Opciones de configuración del DbContext</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet para la entidad Destination
        /// Representa la tabla de destinos turísticos en la base de datos
        /// </summary>
        public DbSet<Destination> Destinations { get; set; }

        /// <summary>
        /// Método llamado durante la creación del modelo para configurar entidades
        /// Define índices para optimizar consultas de filtrado
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo de Entity Framework</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de índices para mejorar el rendimiento en consultas de filtrado
            modelBuilder.Entity<Destination>(entity =>
            {
                entity.HasIndex(e => e.CountryCode);
                entity.HasIndex(e => e.Type);
                entity.HasIndex(e => e.LastModif);
            });
        }
    }
}
