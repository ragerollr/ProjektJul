using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projekt.Data.Identity;
namespace Projekt.Data.Models
{
    public class Projects
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Du måste ange projektets titel!")]
        [StringLength(100, ErrorMessage = "Titeln får inte överstiga 100 tecken.")]
        public string Title { get; set; } = string.Empty; 
        [StringLength(1000, ErrorMessage = "Beskrivningen får inte överstiga 1000 tecken.")]
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.Now;
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<ApplicationUser> Collaborators { get; set; } = new List<ApplicationUser>();
    }
}
