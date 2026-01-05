using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projekt.Data.Identity;

namespace Projekt.Data.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ToUserId { get; set; } = "";

        [ForeignKey(nameof(ToUserId))]
        public ApplicationUser? ToUser { get; set; }

        public string? FromUserId { get; set; }

        [ForeignKey(nameof(FromUserId))]
        public ApplicationUser? FromUser { get; set; }

        [StringLength(80)]
        public string? FromName { get; set; } // används för anonym eller som visningsnamn

        [Required]
        [StringLength(2000)]
        public string Body { get; set; } = "";

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}
