using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CayGiaPha.Models
{
    public class People
    {
        [Key]
        public int PersonalID { get; set; }

        [Required]
        public int FamilyID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime? DeathDate { get; set; }

        [StringLength(200)]
        public string BirthPlace { get; set; }

        [StringLength(500)]
        public string PhotoUrl { get; set; }

        public int? Generation { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("FamilyID")]
        public Families Family { get; set; }

        public ICollection<ParentChild> ParentChildren { get; set; }
        public ICollection<ParentChild> ChildParents { get; set; }
        public ICollection<Marriages> Marriages1 { get; set; }
        public ICollection<Marriages> Marriages2 { get; set; }
        public ICollection<Events> OrganizedEvents { get; set; }
        public ICollection<EventRegistration> EventRegistrations { get; set; }
        public Users User { get; set; }
    }
}
