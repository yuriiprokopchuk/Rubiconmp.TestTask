using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Polygon
    {
        [Required]
        public required Coordinate[] Coordinates { get; set; }
    }
}
