namespace Projekt.Web.ViewModels
{
    public class ProjektModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string DetailUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
