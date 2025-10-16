using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CayGiaPha.Models
{
    public class Events
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(50)]
        public string EventType { get; set; }

        public DateTime? EventDate { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public string Description { get; set; }

        [StringLength(500)]
        public string ImageURL { get; set; }

        public int? OrganizerID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("OrganizerID")]
        public People Organizer { get; set; }

        public ICollection<EventRegistration> Registrations { get; set; }
    }
}
