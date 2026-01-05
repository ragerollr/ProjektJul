using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class SearchVm
    {
        [Required]
        [StringLength(80)]
        public string Query { get; set; } = "";
    }
}
