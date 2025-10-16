using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CayGiaPha.Models
{
    public class Marriages
    {
        [Key]
        public int MarriageID { get; set; }

        [Required]
        public int Spouse1ID { get; set; }

        [Required]
        public int Spouse2ID { get; set; }

        public DateTime? StartDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        // Navigation properties
        [ForeignKey("Spouse1ID")]
        public People Spouse1 { get; set; }

        [ForeignKey("Spouse2ID")]
        public People Spouse2 { get; set; }
    }
}
