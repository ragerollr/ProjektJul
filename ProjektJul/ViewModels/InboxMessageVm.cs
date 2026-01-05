namespace Projekt.Web.ViewModels
{
    public class InboxMessageVm
    {
        public int Id { get; set; }
        public string FromDisplayName { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
