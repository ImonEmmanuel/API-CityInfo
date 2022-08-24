using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CItyInfo.API.Entity
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [ForeignKey("CityId")]
        public City? City { get; set; }
        public int CityId { get; set;  }
        public string? Description { get; set; }
    }
}
