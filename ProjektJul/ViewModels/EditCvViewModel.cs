using CVSite.Data.Models;
using Microsoft.AspNetCore.Http;
using Projekt.Data.Models;
using System.Collections.Generic;
using static Projekt.Web.ViewModels.ProjectCheckBoxViewModel;

namespace Projekt.Web.ViewModels
{
    public class EditCvViewModel
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Address { get; set; } = "";
        public bool IsPublic { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public List<ProjectCheckboxViewModel> Projects { get; set; } = new();

        public List<Utbildning> Utbildningar { get; set; } = new();
        public List<Erfarenhet> Erfarenheter { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
    }
}
