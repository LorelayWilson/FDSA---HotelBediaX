using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationsController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        /// <summary>
        /// Obtiene todos los destinos con filtros y paginación
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<DestinationDto>>> GetDestinations(
            [FromQuery] DestinationFilterDto filter)
        {
            try
            {
                var result = await _destinationService.GetDestinationsAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un destino por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DestinationDto>> GetDestination(int id)
        {
            try
            {
                var destination = await _destinationService.GetDestinationByIdAsync(id);
                
                if (destination == null)
                    return NotFound(new { message = "Destino no encontrado" });

                return Ok(destination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo destino
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DestinationDto>> CreateDestination(CreateDestinationDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var destination = await _destinationService.CreateDestinationAsync(createDto);
                return CreatedAtAction(nameof(GetDestination), new { id = destination.ID }, destination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un destino existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<DestinationDto>> UpdateDestination(int id, UpdateDestinationDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var destination = await _destinationService.UpdateDestinationAsync(id, updateDto);
                
                if (destination == null)
                    return NotFound(new { message = "Destino no encontrado" });

                return Ok(destination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un destino
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDestination(int id)
        {
            try
            {
                var result = await _destinationService.DeleteDestinationAsync(id);
                
                if (!result)
                    return NotFound(new { message = "Destino no encontrado" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la lista de códigos de países disponibles
        /// </summary>
        [HttpGet("countries")]
        public async Task<ActionResult<List<string>>> GetCountries()
        {
            try
            {
                var countries = await _destinationService.GetCountriesAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la lista de tipos de destino disponibles
        /// </summary>
        [HttpGet("types")]
        public ActionResult<List<string>> GetDestinationTypes()
        {
            try
            {
                var types = Enum.GetValues<backend.Models.DestinationType>()
                    .Select(t => t.ToString())
                    .ToList();
                return Ok(types);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
