using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projekt.Data.Identity;

namespace Projekt.Data.Models
{
    public class Erfarenhet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ange företagets namn.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Ange din position/titel.")]
        public string Position { get; set; }

        [StringLength(500, ErrorMessage = "Beskrivningen får inte överstiga 500 tecken")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; } // Kan vara null ifall man fortfarande jobbar där. 
        
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
