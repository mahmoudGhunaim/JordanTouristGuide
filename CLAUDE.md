# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Run Commands

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run

# Build without running
dotnet build

# Database setup (requires EF Core tools)
dotnet tool update --global dotnet-ef
dotnet ef migrations add <MigrationName>
dotnet ef database update

# Reset database
dotnet ef database drop
dotnet ef database update
```

The application runs at `https://localhost:5001` or `http://localhost:5000`.

## Architecture

This is an ASP.NET Core MVC application (.NET 8.0) for a Jordan tourism website with hotel browsing and booking functionality.

### Data Flow

- **Hotel data**: Currently mock data in `PropertyController.GetHotelsFromApi()` - designed to be replaced with real API calls
- **Bookings/Contacts**: Stored in SQL Server via Entity Framework Core, managed through `ApplicationDbContext`

### Key Patterns

- Controllers use constructor injection for `ApplicationDbContext`
- Form submissions use `TempData` for success/error messages
- Booking modal in Property/Detail view submits to `PropertyController.BookNow`

### Database

Connection string in `appsettings.json` uses SQL Server LocalDB by default. Two tables:
- `Bookings` - Hotel booking requests
- `Contacts` - Contact form submissions

Both tables have auto-generated timestamps via `GETDATE()`.

## Design System

Brand colors defined in `wwwroot/css/site.css`:
- Primary Gold: `#ac9576`
- Primary Dark: `#222933`

Custom CSS without Bootstrap - maintain this approach when adding styles.
