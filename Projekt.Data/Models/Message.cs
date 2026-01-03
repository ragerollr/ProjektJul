using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projekt.Data.Identity;

namespace Projekt.Data.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        // Inloggad användare
        public string? SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public ApplicationUser? Sender { get; set; }

        // Används av anonym användare
        [StringLength(100)]
        public string? SenderName { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public ApplicationUser Receiver { get; set; } = null!;

        [Required, StringLength(1000)]
        public string Content { get; set; } = "";

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}
