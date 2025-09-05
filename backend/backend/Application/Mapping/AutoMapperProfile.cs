using AutoMapper;
using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Mapping
{
    /// <summary>
    /// Perfil de configuración de AutoMapper para la aplicación HotelBediaX
    /// Define todas las reglas de mapeo entre entidades y DTOs
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor que configura los mapeos automáticos
        /// </summary>
        public AutoMapperProfile()
        {
            // Mapeo de entidad Destination a DestinationDto
            // AutoMapper mapea automáticamente las propiedades con el mismo nombre
            CreateMap<Destination, DestinationDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
            
            // Mapeo de CreateDestinationDto a entidad Destination
            // Se usa para crear nuevos destinos
            CreateMap<CreateDestinationDto, Destination>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID se asigna automáticamente por la base de datos
                .ForMember(dest => dest.LastModif, opt => opt.Ignore()); // LastModif se asigna en el servicio
            
            // Mapeo de UpdateDestinationDto a entidad Destination
            // Se usa para actualizar destinos existentes
            CreateMap<UpdateDestinationDto, Destination>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID no se debe cambiar en actualizaciones
                .ForMember(dest => dest.LastModif, opt => opt.Ignore()); // LastModif se actualiza en el servicio
        }
    }
}
