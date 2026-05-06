using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace individueelProject.Repository.Models
{
    public class Object2D
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("EnvironmentId")]
        public Guid EnvironmentId { get; set; }
 

        [Required]
        [StringLength(50)]
        public string PrefabId { get; set; }

        [Required]
        public double PostionX { get; set; }

        [Required]
        public double PostionY { get; set; }

        [Required]
        public double ScaleX { get; set; }

        [Required]
        public double ScaleY { get; set; }

        [Required]
        public double RotationZ { get; set; }

        [Required]
        public int SortingLayer { get; set; }
    }
}
