using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StARKS.Models
{
    [Index(nameof(CourseCode), nameof(StudentId), IsUnique = true)]
    public class Mark
    {
        public int Id { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        public int  StudentId { get; set; }

        public int? MarkValue { get; set; }
    }
}
