using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class CreateErfarenhetViewModel
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Position { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
