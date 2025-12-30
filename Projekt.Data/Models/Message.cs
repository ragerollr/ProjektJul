using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Data.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; } // Id på avsändaren
        [ForeignKey(nameof(SenderId))]
        public virtual IdentityUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; } // Id på mottagaren
        [ForeignKey(nameof(ReceiverId))]
        public virtual IdentityUser Receiver { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}