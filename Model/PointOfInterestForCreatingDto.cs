using System.ComponentModel.DataAnnotations;

namespace CItyInfo.API.Model
{
    public class PointOfInterestForCreatingDto
    {
        [Required(ErrorMessage = "Provide a Name.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }
    }
}
