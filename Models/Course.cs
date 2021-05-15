using System.ComponentModel.DataAnnotations;

namespace StARKS.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
