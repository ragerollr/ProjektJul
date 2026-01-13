using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projekt.Data.Identity;


namespace Projekt.Data.Models
{
    public class Utbildning
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ange skolans namn.")]
        public string SchoolName { get; set; }
        
        [Required(ErrorMessage = "Ange namnet på utbildningen.")]
        public string Degree { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Slutdatum")]
        public DateTime? EndDate { get; set; } //Kan vara null ifall man har en pågående utbildning.
       
        
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        
       
        public virtual ApplicationUser User { get; set; }
    }
}
