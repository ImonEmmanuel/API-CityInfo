using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CItyInfo.API.Entity
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }
        public ICollection<PointOfInterest> PointOfInterests { get; set; } = new List<PointOfInterest>();
    }

}
