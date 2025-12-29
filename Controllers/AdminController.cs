using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JordanTouristGuide.Data;

namespace JordanTouristGuide.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string AdminPassword = "mahmoudghunaim";
        private const string SessionKey = "IsAdminLoggedIn";

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString(SessionKey) == "true";
        }

        public IActionResult Login()
        {
            if (IsAuthenticated())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string password)
        {
            if (password == AdminPassword)
            {
                HttpContext.Session.SetString(SessionKey, "true");
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Invalid password. Please try again.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKey);
            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login");
            }

            ViewBag.BookingsCount = await _context.Bookings.CountAsync();
            ViewBag.ContactsCount = await _context.Contacts.CountAsync();
            return View();
        }

        public async Task<IActionResult> Bookings()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var bookings = await _context.Bookings
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return View(bookings);
        }

        public async Task<IActionResult> Contacts()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var contacts = await _context.Contacts
                .OrderByDescending(c => c.SubmittedDate)
                .ToListAsync();
            return View(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking deleted successfully.";
            }
            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(int id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Contact message deleted successfully.";
            }
            return RedirectToAction("Contacts");
        }
    }
}
