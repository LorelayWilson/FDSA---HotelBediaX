using backend.Repositories;

namespace backend.UnitOfWork
{
    /// <summary>
    /// Interfaz para el patrón Unit of Work
    /// Coordina las transacciones y mantiene la consistencia de datos
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de destinos
        /// </summary>
        IDestinationRepository Destinations { get; }

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// </summary>
        /// <returns>Número de registros afectados</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia una nueva transacción
        /// </summary>
        /// <returns>Transacción de base de datos</returns>
        Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
