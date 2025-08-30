using Microsoft.EntityFrameworkCore.Storage;
using backend.Data;
using backend.Repositories;

namespace backend.UnitOfWork
{
    /// <summary>
    /// Implementación del patrón Unit of Work
    /// Coordina las transacciones y mantiene la consistencia de datos
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private IDestinationRepository? _destinations;

        public UnitOfWork(ApplicationDbContext context)
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
        /// Inicia una nueva transacción
        /// </summary>
        /// <returns>Transacción de base de datos</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Libera los recursos utilizados
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
