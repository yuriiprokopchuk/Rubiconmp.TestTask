using Api.Models;
using AutoMapper;
using Services.Dtos;
using Services.Models;

namespace Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CreatePolygon, CreatePolygonDto>()
            .ForMember(d => d.CoordinateDtos, s => s.MapFrom(x => x.Coordinates));

            CreateMap<PolygonDto, Models.Polygon>()
            .ForMember(d => d.Coordinates, s => s.MapFrom(x => x.CoordinateDtos));

            CreateMap<Coordinate, CoordinateDto>();

            CreateMap<CoordinateDto, Coordinate>();
        }
    }
}
