using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CVSite.Data.Models
{
    public class Skill
    {
        [Key]   
        public int Id { get; set; }
        [Required(ErrorMessage = "Ange namn! t.ex C#, JavaScript etc.")]
        [StringLength(50)]
        public string Name { get; set; } //Namn på skillen t.ex C#, Java, Projektledning etc.
        [Range(1, 5)]
        public int Level { get; set; } // Hur bra man är på en skala 1-5
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
