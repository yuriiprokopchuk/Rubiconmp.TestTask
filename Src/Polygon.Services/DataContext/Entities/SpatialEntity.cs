using NetTopologySuite.Geometries;

namespace Services.DataContext.Entities
{
    public class SpatialEntity
    {
        public int Id { get; set; }

        public Geometry Polygon { get; set; }
    }
}
