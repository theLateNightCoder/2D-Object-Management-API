using System.ComponentModel.DataAnnotations;

namespace individueelProject.Repository.Models
{
    public class Environment2D
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(450)]
        public string OwnerUserId { get; set; }

        [Required]
        public int MaxLength { get; set; }

        [Required]
        public int MaxHeight { get; set; }
 
        public ICollection<Object2D> Objects { get; set; } = new List<Object2D>();
    }
}
