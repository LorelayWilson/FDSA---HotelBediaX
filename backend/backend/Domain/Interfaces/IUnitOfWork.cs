namespace backend.Domain.Interfaces
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
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        Task RollbackTransactionAsync();
    }

    /// <summary>
    /// Interfaz para transacciones de base de datos
    /// Abstrae la implementación específica de Entity Framework
    /// </summary>
    public interface IDbContextTransaction : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Confirma la transacción
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Revierte la transacción
        /// </summary>
        Task RollbackAsync();
    }
}
