using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Implementation;
using Services.DataContext;
using Services.DataContext.Entities;
using Services.Dtos;
using Services.Models;
using Coordinate = NetTopologySuite.Geometries.Coordinate;

namespace Services
{
    public class PolygonService
    {
        public const int Srid = 4326;
        private readonly SpatialDbContext _dbContext;

        public PolygonService(SpatialDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreatePolygon(CreatePolygonDto createPolygonDto)
        {
            var coordinates = createPolygonDto.CoordinateDtos.Select(u => new Coordinate(u.X, u.Y)).ToList();
            coordinates.Add(coordinates.First());  // Closing the polygon

            var polygon = new NetTopologySuite.Geometries.Polygon(
                new LinearRing([.. coordinates]),
                new GeometryFactory(new PrecisionModel(), Srid));

            var spatialEntity = new SpatialEntity { Polygon = polygon };

            _dbContext.SpatialEntities.Add(spatialEntity);

            _dbContext.SaveChanges();
        }

        public PolygonDto[] GetPolygons(CoordinateDto[]? coordinateDtos = null)
        {
            var spatialEntities = _dbContext.SpatialEntities.AsQueryable();

            if (coordinateDtos?.Length == 2)
            {
                var segmentCoordinates = coordinateDtos.Select(u => new Coordinate(u.X, u.Y)).ToArray();

                var segment = new LineString(
                    new CoordinateArraySequence(segmentCoordinates),
                    new GeometryFactory(new PrecisionModel(), Srid));

                spatialEntities = spatialEntities.Where(u => u.Polygon.Intersects(segment));
            }

            return spatialEntities.AsEnumerable()
                .Select(u => new PolygonDto
                {
                    CoordinateDtos = u.Polygon.Coordinates
                    .Take(u.Polygon.Coordinates.Length - 1)
                    .Select(n => new CoordinateDto
                    {
                        X = n.X,
                        Y = n.Y
                    }).ToArray()
                }).ToArray();
        }
    }
}
