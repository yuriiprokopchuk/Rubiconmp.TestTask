using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Coordinate
    {
        [Required(ErrorMessage = "X coordinate is required")]
        public required double X { get; set; }

        [Required(ErrorMessage = "Y coordinate is required")]
        public required double Y { get; set; }
    }
}
