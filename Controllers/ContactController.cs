using Microsoft.AspNetCore.Mvc;
using JordanTouristGuide.Models;
using JordanTouristGuide.Data;

namespace JordanTouristGuide.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromForm] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thank you for contacting us! We will get back to you within 24 hours.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Please fill in all required fields correctly.";
            return View("Index", contact);
        }
    }
}
