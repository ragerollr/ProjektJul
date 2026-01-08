using CVSite.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static Projekt.Web.ViewModels.ProjectCheckBoxViewModel;

namespace Projekt.Web.ViewModels
    
{
    public class EditCvViewModel
    {
        // Profilinfo
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool IsPublic { get; set; }



        // CV-uppgifter
        //public string Skills { get; set; }
        //public string Education { get; set; }
        //public string Experience { get; set; }

        //Profilbild
        public IFormFile ProfileImage { get; set; }

        //Project

        public List<ProjectCheckboxViewModel> Projects { get; set; }

        public List<Utbildning> Utbildningar { get; set; }
        public List<Erfarenhet> Erfarenheter { get; set; }
        public List<Skill> Skills { get; set; }



    }
}
