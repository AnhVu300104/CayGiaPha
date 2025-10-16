using System.ComponentModel.DataAnnotations;

namespace CayGiaPha.Models
{
    public class Families
    {
        [Key]
        public int FamilyID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string OriginalPlace { get; set; }

        [StringLength(100)]
        public string BranchName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<People> People { get; set; }
    }
}
