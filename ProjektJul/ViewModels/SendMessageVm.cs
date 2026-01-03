using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class SendMessageVm
    {
        // Om användaren är anonym måste de skriva namn
        [StringLength(100)]
        [Display(Name = "Ditt namn")]
        public string? SenderName { get; set; }

        // För mottagaren
        [Required]
        public string ReceiverId { get; set; } = "";

        [Required]
        [StringLength(1000)]
        [Display(Name = "Meddelande")]
        public string Content { get; set; } = "";
    }
}
