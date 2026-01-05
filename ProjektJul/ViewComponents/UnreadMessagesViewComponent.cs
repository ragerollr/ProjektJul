using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Persistence;
using System.Security.Claims;

namespace Projekt.Web.ViewComponents
{
    public class UnreadMessagesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public UnreadMessagesViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!(User.Identity?.IsAuthenticated ?? false))
                return View(0);

            var myId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId))
                return View(0);

            var unreadCount = await _db.Messages
                .Where(m => m.ToUserId == myId && !m.IsRead)
                .CountAsync();

            return View(unreadCount);
        }
    }
}
