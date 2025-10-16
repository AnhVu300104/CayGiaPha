using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CayGiaPha.Models
{
    public class Users : IdentityUser<int>
    {
        [StringLength(20)]
        public string? Role { get; set; }

        public int? PersonalID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; }

        // Navigation property
        [ForeignKey("PersonalID")]
        public People? Person { get; set; }

        // Override PasswordHash to use plain text
        public override string? PasswordHash
        {
            get => Password;
            set => Password = value;
        }

        public string Password { get; set; } // Plain text password
    }
}
