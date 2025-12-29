using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JordanTouristGuide.Data;
using JordanTouristGuide.Models;

namespace JordanTouristGuide.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // User Dashboard - Shows user's bookings
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var bookings = await _context.Bookings
                .Where(b => b.UserId == user.Id || b.Email == user.Email)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewBag.User = user;
            return View(bookings);
        }

        // Admin Dashboard - Overview
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var stats = new AdminDashboardViewModel
            {
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalContacts = await _context.Contacts.CountAsync(),
                TotalProperties = await _context.Properties.CountAsync(),
                TotalExperiences = await _context.Experiences.CountAsync(),
                TotalUsers = await _userManager.Users.CountAsync(),
                RecentBookings = await _context.Bookings
                    .OrderByDescending(b => b.BookingDate)
                    .Take(5)
                    .ToListAsync()
            };
            return View(stats);
        }

        // Properties Management
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Properties()
        {
            var properties = await _context.Properties
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(properties);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddProperty()
        {
            return View(new Property());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProperty(Property property)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                property.CreatedByUserId = user?.Id;
                property.CreatedAt = DateTime.Now;

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Property added successfully!";
                return RedirectToAction(nameof(Properties));
            }
            return View(property);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null) return NotFound();
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProperty(Property property)
        {
            if (ModelState.IsValid)
            {
                _context.Properties.Update(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Property updated successfully!";
                return RedirectToAction(nameof(Properties));
            }
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Property deleted successfully!";
            }
            return RedirectToAction(nameof(Properties));
        }

        // Experiences Management
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Experiences()
        {
            var experiences = await _context.Experiences
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
            return View(experiences);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddExperience()
        {
            return View(new ExperienceDb());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddExperience(ExperienceDb experience)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                experience.CreatedByUserId = user?.Id;
                experience.CreatedAt = DateTime.Now;

                _context.Experiences.Add(experience);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Experience added successfully!";
                return RedirectToAction(nameof(Experiences));
            }
            return View(experience);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null) return NotFound();
            return View(experience);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditExperience(ExperienceDb experience)
        {
            if (ModelState.IsValid)
            {
                _context.Experiences.Update(experience);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Experience updated successfully!";
                return RedirectToAction(nameof(Experiences));
            }
            return View(experience);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience != null)
            {
                _context.Experiences.Remove(experience);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Experience deleted successfully!";
            }
            return RedirectToAction(nameof(Experiences));
        }

        // Import static properties to database
        [Authorize(Roles = "Admin")]
        public IActionResult ImportStaticData()
        {
            var staticHotels = GetStaticHotels();
            var staticExperiences = GetStaticExperiences();

            ViewBag.Hotels = staticHotels;
            ViewBag.Experiences = staticExperiences;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportHotels(List<string> selectedHotels)
        {
            var user = await _userManager.GetUserAsync(User);
            var staticHotels = GetStaticHotels();
            int imported = 0;

            foreach (var hotelId in selectedHotels)
            {
                var hotel = staticHotels.FirstOrDefault(h => h.Id == hotelId);
                if (hotel != null)
                {
                    var existingProperty = await _context.Properties
                        .FirstOrDefaultAsync(p => p.Name == hotel.Name);

                    if (existingProperty == null)
                    {
                        var property = new Property
                        {
                            Name = hotel.Name,
                            Description = hotel.Description,
                            Location = hotel.Location,
                            ImageUrl = hotel.ImageUrl,
                            PricePerNight = hotel.PricePerNight,
                            Rating = hotel.Rating,
                            Address = hotel.Address,
                            Amenities = string.Join(", ", hotel.Amenities),
                            CreatedByUserId = user?.Id,
                            IsActive = true
                        };
                        _context.Properties.Add(property);
                        imported++;
                    }
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"{imported} properties imported successfully!";
            return RedirectToAction(nameof(ImportStaticData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportExperiences(List<int> selectedExperiences)
        {
            var user = await _userManager.GetUserAsync(User);
            var staticExperiences = GetStaticExperiences();
            int imported = 0;

            foreach (var expId in selectedExperiences)
            {
                var exp = staticExperiences.FirstOrDefault(e => e.Id == expId);
                if (exp != null)
                {
                    var existingExp = await _context.Experiences
                        .FirstOrDefaultAsync(e => e.Title == exp.Title);

                    if (existingExp == null)
                    {
                        var experience = new ExperienceDb
                        {
                            Title = exp.Title,
                            Description = exp.Description,
                            Location = exp.Location,
                            ImageUrl = exp.ImageUrl,
                            Duration = exp.Duration,
                            Category = exp.Category,
                            CreatedByUserId = user?.Id,
                            IsActive = true
                        };
                        _context.Experiences.Add(experience);
                        imported++;
                    }
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"{imported} experiences imported successfully!";
            return RedirectToAction(nameof(ImportStaticData));
        }

        // Manage Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    FullName = user.FullName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    CreatedAt = user.CreatedAt,
                    Roles = roles.ToList()
                });
            }

            return View(userList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                TempData["Success"] = $"{user.Email} is no longer an admin.";
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["Success"] = $"{user.Email} is now an admin.";
            }

            return RedirectToAction(nameof(Users));
        }

        // Bookings Management
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking status updated!";
            }
            return RedirectToAction(nameof(Bookings));
        }

        private List<Hotel> GetStaticHotels()
        {
            return new List<Hotel>
            {
                new Hotel
                {
                    Id = "petra-marriott",
                    Name = "Petra Marriott Hotel",
                    Description = "Experience luxury near the ancient city of Petra. This 5-star hotel offers stunning mountain views, spacious rooms, and world-class amenities.",
                    Location = "Petra",
                    ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=800",
                    PricePerNight = 120,
                    Rating = 4.7,
                    Address = "Tourism Street, Wadi Musa, Petra",
                    Amenities = new List<string> { "Free WiFi", "Swimming Pool", "Restaurant", "Spa", "Free Parking", "Airport Shuttle" }
                },
                new Hotel
                {
                    Id = "wadi-rum-luxury-camp",
                    Name = "Wadi Rum Luxury Camp",
                    Description = "Immerse yourself in the beauty of the Martian-like landscape. Our luxury desert camp offers an authentic Bedouin experience with modern comforts.",
                    Location = "Wadi Rum",
                    ImageUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=800",
                    PricePerNight = 95,
                    Rating = 4.8,
                    Address = "Wadi Rum Desert",
                    Amenities = new List<string> { "Traditional Meals", "Desert Tours", "Stargazing", "Campfire", "Private Bathroom" }
                },
                new Hotel
                {
                    Id = "kempinski-dead-sea",
                    Name = "Kempinski Hotel Ishtar Dead Sea",
                    Description = "Luxurious resort on the shores of the Dead Sea. Enjoy private beaches, infinity pools, and rejuvenating spa treatments with therapeutic Dead Sea products.",
                    Location = "Dead Sea",
                    ImageUrl = "https://images.unsplash.com/photo-1582719508461-905c673771fd?w=800",
                    PricePerNight = 180,
                    Rating = 4.9,
                    Address = "Sweimeh, Dead Sea Road",
                    Amenities = new List<string> { "Private Beach", "Spa & Wellness", "Multiple Pools", "Fine Dining", "Free WiFi", "Kids Club" }
                },
                new Hotel
                {
                    Id = "amman-rotana",
                    Name = "Amman Rotana",
                    Description = "Modern luxury in the heart of Jordan's capital. Perfect for business and leisure travelers seeking comfort and convenience in Amman.",
                    Location = "Amman",
                    ImageUrl = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=800",
                    PricePerNight = 110,
                    Rating = 4.6,
                    Address = "Al-Mutanabi Street, Amman",
                    Amenities = new List<string> { "Free WiFi", "Fitness Center", "Business Center", "Restaurant", "Room Service", "City Views" }
                },
                new Hotel
                {
                    Id = "movenpick-aqaba",
                    Name = "Movenpick Resort & Spa Tala Bay Aqaba",
                    Description = "Beachfront paradise on the Red Sea. Enjoy pristine beaches, crystal-clear waters perfect for diving, and luxurious accommodations.",
                    Location = "Aqaba",
                    ImageUrl = "https://images.unsplash.com/photo-1571896349842-33c89424de2d?w=800",
                    PricePerNight = 140,
                    Rating = 4.7,
                    Address = "Tala Bay, Aqaba",
                    Amenities = new List<string> { "Private Beach", "Diving Center", "Multiple Restaurants", "Spa", "Pool", "Water Sports" }
                },
                new Hotel
                {
                    Id = "feynan-ecolodge",
                    Name = "Feynan Ecolodge",
                    Description = "Award-winning eco-lodge in Dana Biosphere Reserve. Experience sustainable tourism with candlelit evenings and authentic Bedouin hospitality.",
                    Location = "Dana",
                    ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=800",
                    PricePerNight = 85,
                    Rating = 4.8,
                    Address = "Wadi Feynan, Dana Biosphere Reserve",
                    Amenities = new List<string> { "Eco-Friendly", "Hiking Tours", "Traditional Meals", "Candlelit Dining", "Nature Walks" }
                }
            };
        }

        private List<Experience> GetStaticExperiences()
        {
            return new List<Experience>
            {
                new Experience
                {
                    Id = 1,
                    Title = "Petra by Night",
                    Description = "Experience the magic of Petra illuminated by over 1,500 candles. Walk through the Siq and witness the Treasury glowing under the stars while listening to traditional Bedouin music.",
                    ImageUrl = "https://images.unsplash.com/photo-1579606032821-4e6161c81571?w=800",
                    Location = "Petra",
                    Duration = "2 hours",
                    Category = "Cultural"
                },
                new Experience
                {
                    Id = 2,
                    Title = "Wadi Rum Jeep Safari",
                    Description = "Explore the stunning desert landscape of Wadi Rum in a 4x4 jeep. Visit ancient rock inscriptions, towering sandstone mountains, and experience the silence of the desert.",
                    ImageUrl = "https://images.unsplash.com/photo-1682687220742-aba13b6e50ba?w=800",
                    Location = "Wadi Rum",
                    Duration = "4 hours",
                    Category = "Adventure"
                },
                new Experience
                {
                    Id = 3,
                    Title = "Dead Sea Float & Spa",
                    Description = "Float effortlessly in the mineral-rich waters of the Dead Sea, the lowest point on Earth. Enjoy a natural mud treatment and relax at a world-class spa.",
                    ImageUrl = "https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=800",
                    Location = "Dead Sea",
                    Duration = "Half day",
                    Category = "Wellness"
                },
                new Experience
                {
                    Id = 4,
                    Title = "Jerash Roman Ruins Tour",
                    Description = "Step back in time at one of the best-preserved Roman cities in the world. Walk along ancient colonnaded streets, visit temples, and explore the magnificent amphitheater.",
                    ImageUrl = "https://images.unsplash.com/photo-1564769625905-50e93615e769?w=800",
                    Location = "Jerash",
                    Duration = "3 hours",
                    Category = "Historical"
                },
                new Experience
                {
                    Id = 5,
                    Title = "Dana Nature Reserve Hiking",
                    Description = "Trek through Jordan's largest nature reserve, home to diverse wildlife and stunning canyon views. Discover ancient villages and experience traditional Bedouin hospitality.",
                    ImageUrl = "https://images.unsplash.com/photo-1501555088652-021faa106b9b?w=800",
                    Location = "Dana",
                    Duration = "Full day",
                    Category = "Nature"
                },
                new Experience
                {
                    Id = 6,
                    Title = "Mount Nebo & Madaba Visit",
                    Description = "Visit the holy site where Moses viewed the Promised Land and explore Madaba's famous Byzantine mosaics, including the ancient map of the Holy Land.",
                    ImageUrl = "https://images.unsplash.com/photo-1548013146-72479768bada?w=800",
                    Location = "Madaba",
                    Duration = "4 hours",
                    Category = "Religious"
                }
            };
        }
    }

    public class AdminDashboardViewModel
    {
        public int TotalBookings { get; set; }
        public int TotalContacts { get; set; }
        public int TotalProperties { get; set; }
        public int TotalExperiences { get; set; }
        public int TotalUsers { get; set; }
        public List<Booking> RecentBookings { get; set; } = new();
    }

    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
