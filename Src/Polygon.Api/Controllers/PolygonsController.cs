using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace Api.Controllers
{
    [Route("api/polygons")]
    [ApiController]
    public class PolygonsController : ControllerBase
    {
        private readonly PolygonService _polygonService;
        private readonly IMapper _mapper;

        public PolygonsController(PolygonService polygonService, IMapper mapper)
        {
            _polygonService = polygonService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("segment/{x1}/{y1}/{x2}/{y2}")]
        public ActionResult<Models.Polygon[]> GetPolygonsBySegment(double x1, double y1, double x2, double y2)
        {
            CoordinateDto[] coordinateDtos = [
                new CoordinateDto { X = x1, Y = y1 },
                new CoordinateDto { X = x2, Y = y2 }];

            var polygonDtos = _polygonService.GetPolygons(coordinateDtos);

            return _mapper.Map<Models.Polygon[]>(polygonDtos);
        }

        [HttpPost]
        public ActionResult<int> CreatePolygon(CreatePolygon createPolygon)
        {
            var createPolygonDto = _mapper.Map<CreatePolygonDto>(createPolygon);

            var id = _polygonService.CreatePolygon(createPolygonDto);

            return id;
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeletePolygon(int id)
        {
            _polygonService.DeletePolygon(id);

            return NoContent();
        }
    }
}
