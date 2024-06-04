using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreatePolygon
    {
        [Required]
        public required Coordinate[] Coordinates { get; set; }
    }
}
