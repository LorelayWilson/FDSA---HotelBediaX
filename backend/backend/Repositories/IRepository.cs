using System.Linq.Expressions;

namespace backend.Repositories
{
    /// <summary>
    /// Interfaz genérica para el patrón Repository
    /// Proporciona operaciones CRUD básicas para cualquier entidad
    /// </summary>
    /// <typeparam name="TEntity">Tipo de entidad</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Obtiene una entidad por su ID
        /// </summary>
        /// <param name="id">ID de la entidad</param>
        /// <returns>Entidad encontrada o null</returns>
        Task<TEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        /// <returns>Lista de entidades</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Obtiene entidades que cumplen con una condición
        /// </summary>
        /// <param name="predicate">Condición de filtrado</param>
        /// <returns>Lista de entidades que cumplen la condición</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtiene la primera entidad que cumple con una condición
        /// </summary>
        /// <param name="predicate">Condición de filtrado</param>
        /// <returns>Primera entidad encontrada o null</returns>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Cuenta las entidades que cumplen con una condición
        /// </summary>
        /// <param name="predicate">Condición de filtrado</param>
        /// <returns>Número de entidades que cumplen la condición</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

        /// <summary>
        /// Verifica si existe alguna entidad que cumple con una condición
        /// </summary>
        /// <param name="predicate">Condición de filtrado</param>
        /// <returns>True si existe al menos una entidad que cumple la condición</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Agrega una nueva entidad
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        void Add(TEntity entity);

        /// <summary>
        /// Agrega múltiples entidades
        /// </summary>
        /// <param name="entities">Entidades a agregar</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        void Update(TEntity entity);

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Elimina múltiples entidades
        /// </summary>
        /// <param name="entities">Entidades a eliminar</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
