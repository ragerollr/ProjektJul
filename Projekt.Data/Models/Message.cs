using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Data.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        // Inloggad användare (kan vara null om anonym)
        public string? SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public IdentityUser? Sender { get; set; }

        // För anonym användare
        [StringLength(100)]
        public string? SenderName { get; set; }

        [Required]
        public string ReceiverId { get; set; } = "";

        [ForeignKey(nameof(ReceiverId))]
        public IdentityUser Receiver { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = "";

        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}