namespace Projekt.Web.ViewModels
{
    public class ProjectFormViewModel
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }

    public class ProjectListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public bool OwnerIsPrivate { get; set; }
    }
}
