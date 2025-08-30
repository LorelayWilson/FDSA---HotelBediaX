using Microsoft.EntityFrameworkCore;
using backend.Infrastructure.Data;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Interfaces;
using backend.Application.DTOs;

namespace backend.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación específica del repositorio de destinos
    /// Proporciona operaciones especializadas para la entidad Destination
    /// </summary>
    public class DestinationRepository : Repository<Destination>, IDestinationRepository
    {
        public DestinationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResultDto<Destination>> GetDestinationsWithFiltersAsync(DestinationFilterDto filter)
        {
            var query = _dbSet.AsQueryable();

            // Aplicar filtro de búsqueda por texto
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(d => 
                    d.Name.Contains(filter.SearchTerm) || 
                    d.Description.Contains(filter.SearchTerm) ||
                    d.CountryCode.Contains(filter.SearchTerm));
            }

            // Aplicar filtro por código de país específico
            if (!string.IsNullOrWhiteSpace(filter.CountryCode))
            {
                query = query.Where(d => d.CountryCode == filter.CountryCode);
            }

            // Aplicar filtro por tipo de destino
            if (filter.Type.HasValue)
            {
                query = query.Where(d => d.Type == filter.Type.Value);
            }

            // Obtener el total de registros antes de aplicar paginación
            var totalCount = await query.CountAsync();

            // Aplicar paginación y ordenamiento
            var destinations = await query
                .OrderByDescending(d => d.LastModif)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResultDto<Destination>
            {
                Items = destinations,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<List<string>> GetUniqueCountryCodesAsync()
        {
            return await _dbSet
                .Select(d => d.CountryCode)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<List<Destination>> GetDestinationsByTypeAsync(DestinationType type)
        {
            return await _dbSet
                .Where(d => d.Type == type)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<List<Destination>> SearchDestinationsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(d => 
                    d.Name.Contains(searchTerm) || 
                    d.Description.Contains(searchTerm) ||
                    d.CountryCode.Contains(searchTerm))
                .OrderBy(d => d.Name)
                .ToListAsync();
        }
    }
}
