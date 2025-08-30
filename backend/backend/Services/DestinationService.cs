using Microsoft.EntityFrameworkCore;
using AutoMapper;
using backend.Data;
using backend.DTOs;
using backend.Models;
using Serilog;

namespace backend.Services
{
    /// <summary>
    /// Implementación del servicio de destinos turísticos
    /// Maneja toda la lógica de negocio para las operaciones CRUD de destinos
    /// </summary>
    public class DestinationService : IDestinationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DestinationService> _logger;

        /// <summary>
        /// Constructor que recibe las dependencias necesarias
        /// </summary>
        /// <param name="context">Contexto de Entity Framework para acceso a datos</param>
        /// <param name="mapper">Instancia de AutoMapper para conversiones entre entidades y DTOs</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public DestinationService(ApplicationDbContext context, IMapper mapper, ILogger<DestinationService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene destinos con filtros y paginación
        /// Implementa búsqueda por texto, filtros por país y tipo, y paginación eficiente
        /// </summary>
        public async Task<PagedResultDto<DestinationDto>> GetDestinationsAsync(DestinationFilterDto filter)
        {
            // Inicializar query base
            var query = _context.Destinations.AsQueryable();

            // Aplicar filtro de búsqueda por texto
            // Busca en nombre, descripción y código de país
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
            // Esto es necesario para calcular el número total de páginas
            var totalCount = await query.CountAsync();

            // Aplicar paginación y ordenamiento
            // Ordenamos por fecha de modificación descendente (más recientes primero)
            var destinations = await query
                .OrderByDescending(d => d.LastModif)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            // Mapear entidades a DTOs usando AutoMapper
            var destinationDtos = _mapper.Map<List<DestinationDto>>(destinations);

            // Construir y retornar el resultado paginado
            var result = new PagedResultDto<DestinationDto>
            {
                Items = destinationDtos,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };

            return result;
        }

        /// <summary>
        /// Obtiene un destino específico por su ID
        /// </summary>
        public async Task<DestinationDto?> GetDestinationByIdAsync(int id)
        {
            // Buscar el destino en la base de datos
            var destination = await _context.Destinations
                .FirstOrDefaultAsync(d => d.ID == id);

            // Retornar DTO mapeado o null si no existe
            return destination != null ? _mapper.Map<DestinationDto>(destination) : null;
        }

        /// <summary>
        /// Crea un nuevo destino en el sistema
        /// Asigna automáticamente la fecha de modificación
        /// </summary>
        public async Task<DestinationDto> CreateDestinationAsync(CreateDestinationDto createDto)
        {
            // Mapear DTO a entidad usando AutoMapper
            var destination = _mapper.Map<Destination>(createDto);
            
            // Asignar fecha de modificación automáticamente
            destination.LastModif = DateTime.UtcNow;

            // Agregar a la base de datos y guardar cambios
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            Log.Information("Destino creado: {DestinationName} (ID: {DestinationId}) en {CountryCode}", 
                destination.Name, destination.ID, destination.CountryCode);

            // Retornar DTO del destino creado (con ID asignado)
            return _mapper.Map<DestinationDto>(destination);
        }

        /// <summary>
        /// Actualiza un destino existente
        /// Actualiza automáticamente la fecha de modificación
        /// </summary>
        public async Task<DestinationDto?> UpdateDestinationAsync(int id, UpdateDestinationDto updateDto)
        {
            // Buscar el destino a actualizar
            var destination = await _context.Destinations
                .FirstOrDefaultAsync(d => d.ID == id);

            // Retornar null si no existe
            if (destination == null)
            {
                Log.Warning("Destino no encontrado para actualizar con ID: {DestinationId}", id);
                return null;
            }

            // Aplicar los cambios del DTO a la entidad
            _mapper.Map(updateDto, destination);
            
            // Actualizar fecha de modificación
            destination.LastModif = DateTime.UtcNow;

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            Log.Information("Destino actualizado: {DestinationName} (ID: {DestinationId})", 
                destination.Name, destination.ID);

            // Retornar DTO del destino actualizado
            return _mapper.Map<DestinationDto>(destination);
        }

        /// <summary>
        /// Elimina permanentemente un destino del sistema
        /// </summary>
        public async Task<bool> DeleteDestinationAsync(int id)
        {
            // Buscar el destino a eliminar
            var destination = await _context.Destinations
                .FirstOrDefaultAsync(d => d.ID == id);

            // Retornar false si no existe
            if (destination == null)
            {
                Log.Warning("Destino no encontrado para eliminar con ID: {DestinationId}", id);
                return false;
            }

            // Eliminar de la base de datos
            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            Log.Information("Destino eliminado: {DestinationName} (ID: {DestinationId})", 
                destination.Name, destination.ID);

            return true;
        }

        /// <summary>
        /// Obtiene la lista de códigos de países únicos
        /// Útil para llenar dropdowns en la interfaz de usuario
        /// </summary>
        public async Task<List<string>> GetCountriesAsync()
        {
            return await _context.Destinations
                .Select(d => d.CountryCode)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
}
