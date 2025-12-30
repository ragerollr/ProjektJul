using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Models;
using Projekt.Data.Persistence;

namespace Projekt.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Message
        public IActionResult Index()
        {
            var messages = _context.Messages.ToList();
            return View(messages);
        }

        // POST: /Message/Create
        [HttpPost]
        public IActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }
    }
}
