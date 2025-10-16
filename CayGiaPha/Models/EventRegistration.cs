using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CayGiaPha.Models
{
    public class EventRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public int PersonalID { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("EventID")]
        public virtual Events Event { get; set; }

        [ForeignKey("PersonalID")]
        public virtual People Person { get; set; }
    }
}
