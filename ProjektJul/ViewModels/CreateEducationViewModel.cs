using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class CreateEducationViewModel
    {
        [Required]
        public string SchoolName { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
