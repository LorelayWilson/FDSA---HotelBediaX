using Microsoft.EntityFrameworkCore.Storage;
using backend.Infrastructure.Data;
using backend.Infrastructure.Repositories;
using backend.Domain.Interfaces;
using DomainTransaction = backend.Domain.Interfaces.IDbContextTransaction;
using EfTransaction = Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction;

namespace backend.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implementación del patrón Unit of Work
    /// Coordina las transacciones y mantiene la consistencia de datos
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private DomainTransaction? _transaction;
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
        public async Task<DomainTransaction> BeginTransactionAsync()
        {
            var efTransaction = await _context.Database.BeginTransactionAsync();
            _transaction = new EfDbContextTransaction(efTransaction);
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

    /// <summary>
    /// Adaptador para transacciones de Entity Framework
    /// Implementa la interfaz del dominio
    /// </summary>
    internal class EfDbContextTransaction : DomainTransaction
    {
        private readonly EfTransaction _efTransaction;

        public EfDbContextTransaction(EfTransaction efTransaction)
        {
            _efTransaction = efTransaction;
        }

        public async Task CommitAsync()
        {
            await _efTransaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _efTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            _efTransaction.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _efTransaction.DisposeAsync();
        }
    }
}
