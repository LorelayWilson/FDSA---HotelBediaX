namespace backend.Domain.Interfaces
{
    /// <summary>
    /// Interfaz genérica para resultados paginados
    /// Permite que el dominio no dependa de DTOs específicos de Application
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que se está paginando</typeparam>
    public interface IPagedResult<T>
    {
        /// <summary>Lista de elementos de la página actual</summary>
        List<T> Items { get; set; }
        /// <summary>Número total de elementos que coinciden con los filtros</summary>
        int TotalCount { get; set; }
        /// <summary>Número de página actual</summary>
        int Page { get; set; }
        /// <summary>Número de elementos por página</summary>
        int PageSize { get; set; }
        /// <summary>Número total de páginas calculado automáticamente</summary>
        int TotalPages { get; }
    }
}
