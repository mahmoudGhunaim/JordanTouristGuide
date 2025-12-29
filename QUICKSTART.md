# Quick Start Guide

## Getting Started in 3 Steps

### Step 1: Setup Database

**Option A - Using the Setup Script (Recommended for Windows)**
```bash
setup-database.bat
```

**Option B - Manual Setup**
```bash
# Install EF Core tools (if not already installed)
dotnet tool update --global dotnet-ef

# Create and apply database migration
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 2: Run the Application
```bash
dotnet run
```

### Step 3: Open in Browser
Navigate to: `https://localhost:5001` or `http://localhost:5000`

## What You'll See

1. **Home Page** - Portfolio showcase of Jordan's attractions
2. **Properties** - Browse 6 hotels across different cities in Jordan
3. **Property Details** - Click on any hotel to see details and book
4. **Experiences** - Discover 12 different activities and places to visit
5. **Contact Us** - Send inquiries through the contact form

## Testing the Features

### Test Booking System
1. Go to **Properties** page
2. Click **View Details** on any hotel
3. Click **Book Now** button
4. Fill in the form and submit
5. Your booking will be saved to the database

### Test Contact Form
1. Go to **Contact Us** page
2. Fill in all required fields
3. Submit the form
4. Your message will be saved to the database

## Viewing Database Records

To view the data saved in the database, you can use:
- **SQL Server Management Studio (SSMS)**
- **Visual Studio's SQL Server Object Explorer**
- **Azure Data Studio**

Connect to: `(localdb)\mssqllocaldb`
Database: `JordanTouristGuide`

Tables:
- `Bookings` - Hotel booking requests
- `Contacts` - Contact form submissions

## Troubleshooting

### "Database does not exist" Error
Run: `dotnet ef database update`

### Port Already in Use
Edit `Properties/launchSettings.json` to change the port numbers

### Cannot Connect to Database
Ensure SQL Server LocalDB is installed (comes with Visual Studio)

## Project Colors

The site uses the specified brand colors:
- **Primary Gold**: #ac9576
- **Primary Dark**: #222933

These colors are defined in `wwwroot/css/site.css` as CSS variables.

## Next Steps

- Customize hotel data in `Controllers/PropertyController.cs`
- Add your own images to `wwwroot/images/`
- Integrate with a real hotel booking API
- Add email notifications for bookings
- Implement admin panel to view submissions

Enjoy exploring Jordan! 🇯🇴
