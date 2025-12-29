# Project Features Overview

## Your Tourist Guide in Jordan

This ASP.NET MVC application provides a complete tourism platform for discovering and booking experiences in Jordan.

---

## 📱 Pages & Features

### 1. Home Page (`/`)
**Portfolio-style showcase of Jordan**

Features:
- Hero section with stunning background and call-to-action buttons
- "Why Visit Jordan?" section with 6 destination cards:
  - Ancient Petra
  - Wadi Rum Desert
  - Dead Sea
  - Jerash Ruins
  - Red Sea Diving in Aqaba
  - Modern Amman
- "Plan Your Journey" section with quick links
- Fully responsive design
- Images from Unsplash for professional appearance

**Navigation:**
- Click "Explore Hotels" to go to Properties page
- Click "View Experiences" to go to Experiences page

---

### 2. Properties Page (`/Property`)
**Browse hotels across Jordan**

Features:
- Displays 6 hotels across different locations:
  1. **Petra Marriott Hotel** - Petra ($120/night)
  2. **Wadi Rum Luxury Camp** - Wadi Rum ($95/night)
  3. **Kempinski Hotel Ishtar Dead Sea** - Dead Sea ($180/night)
  4. **Amman Rotana** - Amman ($110/night)
  5. **Movenpick Resort & Spa** - Aqaba ($140/night)
  6. **Feynan Ecolodge** - Dana ($85/night)

Each hotel card shows:
- Professional image
- Hotel name and location
- Star rating
- Brief description
- Price per night
- "View Details" button

**Data Source:**
- Currently uses mock data (easily replaceable with real API)
- Hotel model includes: name, description, location, image, price, rating, amenities

---

### 3. Property Detail Page (`/Property/Detail/{id}`)
**Detailed hotel information with booking**

Features:
- Large hotel image
- Complete hotel description
- Location and address
- Star rating
- Amenity tags (WiFi, Pool, Spa, etc.)
- Price display
- **"Book Now" button** - Opens booking modal

**Booking Modal Popup:**
- Form fields:
  - Full Name (required)
  - Email (required, validated)
  - Phone Number (required)
  - Check-in Date (required, date picker)
  - Check-out Date (required, date picker)
  - Number of Guests (required, 1-10)
  - Special Requests (optional, textarea)

**Backend Integration:**
- Form submission saves to SQL database
- Uses Entity Framework Core
- Success message displayed after submission
- Data stored in `Bookings` table

**Validation:**
- Client-side validation for required fields
- Email format validation
- Phone format validation
- Date validation (check-in before check-out)
- Guest count range (1-10)

---

### 4. Experiences Page (`/Experience`)
**Discover activities and attractions**

Features:
- Grid layout showcasing 12 experiences:
  1. Petra by Night
  2. Wadi Rum Jeep Safari
  3. Dead Sea Floating
  4. Red Sea Diving
  5. Jerash Ancient City Tour
  6. Wadi Mujib Canyon Trek
  7. Bedouin Cultural Experience
  8. Amman Food Tour
  9. Dana Biosphere Reserve
  10. Mount Nebo & Madaba
  11. Wadi Rum Hot Air Balloon
  12. Ajloun Castle & Forest

**Interactive Cards:**
- Hover effect reveals full description
- Image zoom animation on hover
- Overlay with gradient background
- Experience title in gold color
- Detailed description text

**Design:**
- Modern card-based layout
- Professional images
- Smooth animations
- Call-to-action section at bottom

---

### 5. Contact Us Page (`/Contact`)
**Get in touch with inquiries**

Features:
- Professional contact form
- Contact information display (Email, Phone, Office location)
- Icon-based information cards

**Contact Form Fields:**
- Full Name (required)
- Email Address (required, validated)
- Phone Number (required)
- Subject (required)
- Message (required, textarea)

**Backend Integration:**
- Form submission saves to SQL database
- Uses Entity Framework Core
- Success/error messages displayed
- Data stored in `Contacts` table

**Validation:**
- Required field validation
- Email format validation
- Phone number validation
- Client and server-side validation

---

## 🎨 Design & Styling

### Color Scheme
- **Primary Gold**: `#ac9576` - Used for accents, buttons, and highlights
- **Primary Dark**: `#222933` - Used for header, footer, and text
- Light gray for backgrounds
- White for cards and content areas

### Design Features
- Clean, modern interface
- No Bootstrap - custom CSS for unique look
- Fully responsive (mobile, tablet, desktop)
- Smooth animations and transitions
- Professional typography
- Card-based layouts
- Hover effects throughout

### Branding
- Simple text-based logo: "Your Tourist Guide in Jordan"
- Consistent color usage across all pages
- Professional imagery from Unsplash
- Clean navigation menu

---

## 💾 Database Schema

### Bookings Table
```sql
CREATE TABLE Bookings (
    Id INT PRIMARY KEY IDENTITY,
    HotelName NVARCHAR(MAX),
    FullName NVARCHAR(MAX),
    Email NVARCHAR(MAX),
    Phone NVARCHAR(MAX),
    CheckInDate DATETIME2,
    CheckOutDate DATETIME2,
    NumberOfGuests INT,
    SpecialRequests NVARCHAR(MAX),
    BookingDate DATETIME2 DEFAULT GETDATE()
)
```

### Contacts Table
```sql
CREATE TABLE Contacts (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(MAX),
    Email NVARCHAR(MAX),
    Phone NVARCHAR(MAX),
    Subject NVARCHAR(MAX),
    Message NVARCHAR(MAX),
    SubmittedDate DATETIME2 DEFAULT GETDATE()
)
```

---

## 🔧 Technical Stack

- **Framework**: ASP.NET Core MVC (.NET 8.0)
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server (LocalDB for development)
- **Frontend**: Razor Views, Custom CSS, Vanilla JavaScript
- **Validation**: Data Annotations, jQuery Validation
- **API Integration**: Ready for hotel API integration

---

## 🚀 Key Technical Features

1. **MVC Architecture**: Clean separation of concerns
2. **Entity Framework**: Code-first database approach
3. **Responsive Design**: Works on all devices
4. **Form Validation**: Client and server-side
5. **Modal Popups**: For booking without page reload
6. **TempData Messages**: User feedback for actions
7. **SEO-Friendly**: Semantic HTML structure
8. **Performance**: Optimized CSS and minimal JavaScript

---

## 📦 Included Files

### Controllers (4)
- `HomeController.cs` - Home page
- `PropertyController.cs` - Hotel listings and details
- `ExperienceController.cs` - Experiences page
- `ContactController.cs` - Contact form handling

### Models (3)
- `Booking.cs` - Hotel booking data
- `Contact.cs` - Contact form data
- `Hotel.cs` - Hotel information

### Views (8)
- Home/Index.cshtml
- Property/Index.cshtml
- Property/Detail.cshtml
- Experience/Index.cshtml
- Contact/Index.cshtml
- Shared/_Layout.cshtml
- Shared/_ValidationScriptsPartial.cshtml
- _ViewImports.cshtml, _ViewStart.cshtml

### Static Files
- `wwwroot/css/site.css` - All styling (400+ lines)
- `wwwroot/js/site.js` - Interactive features

### Configuration
- `appsettings.json` - Connection string
- `Program.cs` - Application startup
- `JordanTouristGuide.csproj` - Project dependencies

---

## 🎯 Ready for Production

The application is fully functional and ready for:
- ✅ Local development and testing
- ✅ Database integration
- ✅ Form submissions
- ✅ User interactions
- ✅ Responsive design testing

**Next Steps for Production:**
1. Replace mock hotel data with real API
2. Add user authentication
3. Implement payment gateway
4. Add email notifications
5. Deploy to Azure/IIS

---

Enjoy your Jordan tourism application! 🇯🇴✨
