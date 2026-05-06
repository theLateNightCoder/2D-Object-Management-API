using System.ComponentModel.DataAnnotations;

namespace individueelProject.Repository.Models
{
    public class Environment2DDTO
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        public int MaxLength { get; set; }

        [Required]
        public int MaxHeight { get; set; }

        
    }
}
