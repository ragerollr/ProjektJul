using Microsoft.AspNetCore.Mvc.Rendering;

namespace Projekt.Web.ViewModels
{
    public class ProjectFormViewModel //Viewmodel för att skapa och redigera projekt. 
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        public List<string> SelectedCollaboratorIds { get; set; } = new List<string>();
        public List<SelectListItem> AvailableUsers { get; set; } = new List<SelectListItem>();
    }

    public class ProjectListItemViewModel //Viewmodel som visar projekten i listan. 
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public bool OwnerIsPrivate { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public List<string> CollaboratorNames { get; set; } = new List<string>();
    }
}
