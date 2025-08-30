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
        /// Define restricciones, índices y configuraciones específicas
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo de Entity Framework</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración específica de la entidad Destination
            modelBuilder.Entity<Destination>(entity =>
            {
                // Configuración de la clave primaria
                entity.HasKey(e => e.ID);
                
                // Configuración de propiedades con validaciones
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);
                    
                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(3);
                    
                entity.Property(e => e.Type)
                    .IsRequired();
                    
                entity.Property(e => e.LastModif)
                    .IsRequired();

                // Índices para mejorar el rendimiento en consultas de filtrado
                // Estos índices son especialmente importantes para grandes volúmenes de datos
                entity.HasIndex(e => e.CountryCode);
                entity.HasIndex(e => e.Type);
                entity.HasIndex(e => e.LastModif);
            });
        }
    }
}
