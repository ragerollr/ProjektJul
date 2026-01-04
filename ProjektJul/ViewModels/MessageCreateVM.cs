using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class MessageCreateVm
    {
        [Required]
        public string ToUserId { get; set; } = "";

        [StringLength(80)]
        public string? FromName { get; set; }

        [Required]
        [StringLength(2000)]
        public string Body { get; set; } = "";
    }
}
