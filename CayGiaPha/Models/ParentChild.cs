using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CayGiaPha.Models
{
    public class ParentChild
    {
        [Key]
        public int PC_ID { get; set; }

        [Required]
        public int ParentID { get; set; }

        [Required]
        public int ChildID { get; set; }

        [StringLength(20)]
        public string RelationshipType { get; set; }

        // Navigation properties
        [ForeignKey("ParentID")]
        public People Parent { get; set; }

        [ForeignKey("ChildID")]
        public People Child { get; set; }
    }
}
