namespace Projekt.Web.ViewModels
{
    public class IndexVm
    {
        public List<CvModel> FeaturedCvs { get; set; } = new();
        public ProjektModel? LatestProject { get; set; }
    }
}
