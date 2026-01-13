using System.ComponentModel.DataAnnotations;

using Projekt.Data.Identity;


namespace Projekt.Data.Models
{
    public class Skill
    {
        [Key]   
        public int Id { get; set; }
        [Required(ErrorMessage = "Ange namn! t.ex C#, JavaScript etc.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; //Namn på skillen t.ex C#, Java, Projektledning etc.
        [Range(1, 5)]
        public int Level { get; set; } // Hur bra man är på en skala 1-5
        public string UserId { get; set; } = null!;

        
        public ApplicationUser? User { get; set; }
    }
}
