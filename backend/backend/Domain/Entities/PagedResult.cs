using backend.Domain.Interfaces;

namespace backend.Domain.Entities
{
    /// <summary>
    /// Implementación concreta de IPagedResult para resultados paginados
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que se está paginando</typeparam>
    public class PagedResult<T> : IPagedResult<T>
    {
        /// <summary>Lista de elementos de la página actual</summary>
        public List<T> Items { get; set; } = new();
        /// <summary>Número total de elementos que coinciden con los filtros</summary>
        public int TotalCount { get; set; }
        /// <summary>Número de página actual</summary>
        public int Page { get; set; }
        /// <summary>Número de elementos por página</summary>
        public int PageSize { get; set; }
        /// <summary>Número total de páginas calculado automáticamente</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
