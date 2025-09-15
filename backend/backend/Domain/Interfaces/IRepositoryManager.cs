namespace backend.Domain.Interfaces
{
    /// <summary>
    /// Interfaz simple para gestionar repositorios
    /// Reemplaza el UoW redundante con una abstracción más simple
    /// </summary>
    public interface IRepositoryManager : IDisposable
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
    }
}
