using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVSite.Data.Models
{
    public class Projekt
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Du måste ange projektets titel!")]
        [StringLength(100, ErrorMessage = "Titeln får inte överstiga 100 tecken.")]
        public string Title { get; set; }
        [StringLength(1000, ErrorMessage = "Beskrivningen får inte överstiga 1000 tecken.")]
        public string Description { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
