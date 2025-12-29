# Your Tourist Guide in Jordan

A comprehensive ASP.NET MVC web application for discovering and booking tourist experiences in Jordan.

## Features

- **Home Page**: Portfolio-style showcase of Jordan's attractions
- **Properties**: Browse hotels across Jordan with detailed information
- **Property Details**: View hotel information with booking functionality
- **Experiences**: Discover places to visit and activities in Jordan
- **Contact Us**: Get in touch with inquiries

## Technologies Used

- ASP.NET Core MVC (.NET 8.0)
- Entity Framework Core
- SQL Server (LocalDB)
- Bootstrap-free responsive design
- Custom CSS with branded colors

## Color Scheme

- Primary Gold: `#ac9576`
- Primary Dark: `#222933`

## Setup Instructions

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code
- SQL Server LocalDB (included with Visual Studio)

### Installation Steps

1. **Navigate to the project directory**
   ```bash
   cd JordanTouristGuide
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Create the database**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open in browser**
   - Navigate to: `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
JordanTouristGuide/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── PropertyController.cs
│   ├── ExperienceController.cs
│   └── ContactController.cs
├── Models/              # Data models
│   ├── Booking.cs
│   ├── Contact.cs
│   └── Hotel.cs
├── Views/               # Razor views
│   ├── Home/
│   ├── Property/
│   ├── Experience/
│   ├── Contact/
│   └── Shared/
├── Data/                # Database context
│   └── ApplicationDbContext.cs
└── wwwroot/             # Static files
    ├── css/
    ├── js/
    └── images/
```

## Database Schema

### Bookings Table
- Id (Primary Key)
- HotelName
- FullName
- Email
- Phone
- CheckInDate
- CheckOutDate
- NumberOfGuests
- SpecialRequests
- BookingDate

### Contacts Table
- Id (Primary Key)
- FullName
- Email
- Phone
- Subject
- Message
- SubmittedDate

## Usage

### Browsing Hotels
1. Navigate to "Properties" from the navigation menu
2. Browse available hotels across Jordan
3. Click "View Details" to see more information about a specific hotel

### Booking a Hotel
1. On the hotel detail page, click "Book Now"
2. Fill in the booking form with your details
3. Submit the form - your booking request will be saved to the database

### Exploring Experiences
1. Navigate to "Experiences" from the navigation menu
2. Hover over cards to see detailed descriptions
3. Discover various attractions and activities across Jordan

### Contacting Us
1. Navigate to "Contact Us" from the navigation menu
2. Fill in the contact form
3. Submit your inquiry - it will be saved to the database

## Development Notes

### Hotel Data
Currently, the application uses mock hotel data. To integrate with a real hotel API:
1. Open `Controllers/PropertyController.cs`
2. Replace the `GetHotelsFromApi()` method with actual API calls
3. Update the `Hotel` model if needed to match API response structure

### Database Connection
The connection string is configured in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JordanTouristGuide;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

To use a different SQL Server instance, update this connection string.

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server LocalDB is installed
- Check if the connection string in `appsettings.json` is correct
- Try recreating the database: `dotnet ef database drop` then `dotnet ef database update`

### Migration Issues
- Delete the `Migrations` folder
- Run: `dotnet ef migrations add InitialCreate`
- Run: `dotnet ef database update`

## Future Enhancements

- Integrate with real hotel booking API
- Add user authentication and authorization
- Implement payment gateway integration
- Add admin panel for managing bookings and contacts
- Add email notifications for bookings and contact submissions
- Implement multi-language support (English/Arabic)
- Add user reviews and ratings

## License

This project is created for educational and commercial purposes.

## Contact

For questions or support, please use the Contact Us page in the application.
