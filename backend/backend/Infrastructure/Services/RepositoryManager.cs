using backend.Infrastructure.Data;
using backend.Infrastructure.Repositories;
using backend.Domain.Interfaces;

namespace backend.Infrastructure.Services
{
    /// <summary>
    /// Implementación simple del gestor de repositorios
    /// Reemplaza el UoW redundante con una abstracción más directa
    /// </summary>
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;
        private IDestinationRepository? _destinations;

        public RepositoryManager(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Repositorio de destinos (lazy initialization)
        /// </summary>
        public IDestinationRepository Destinations
        {
            get
            {
                _destinations ??= new DestinationRepository(_context);
                return _destinations;
            }
        }

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// </summary>
        /// <returns>Número de registros afectados</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Libera los recursos utilizados
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
