using backend.Application.DTOs;
using backend.Domain.Interfaces;

namespace backend.Application.Adapters
{
    /// <summary>
    /// Adaptador para convertir entre resultados paginados del dominio y DTOs
    /// Mantiene la separación entre capas de Clean Architecture
    /// </summary>
    public static class PagedResultAdapter
    {
        /// <summary>
        /// Convierte IPagedResult del dominio a PagedResultDto de la aplicación
        /// </summary>
        /// <typeparam name="TEntity">Tipo de entidad del dominio</typeparam>
        /// <typeparam name="TDto">Tipo de DTO de la aplicación</typeparam>
        /// <param name="domainResult">Resultado paginado del dominio</param>
        /// <param name="entityToDtoMapper">Función para mapear entidad a DTO</param>
        /// <returns>Resultado paginado como DTO</returns>
        public static PagedResultDto<TDto> ToDto<TEntity, TDto>(
            IPagedResult<TEntity> domainResult, 
            Func<TEntity, TDto> entityToDtoMapper)
        {
            return new PagedResultDto<TDto>
            {
                Items = domainResult.Items.Select(entityToDtoMapper).ToList(),
                TotalCount = domainResult.TotalCount,
                Page = domainResult.Page,
                PageSize = domainResult.PageSize
            };
        }
    }
}
