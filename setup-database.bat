@echo off
echo ====================================
echo Jordan Tourist Guide - Database Setup
echo ====================================
echo.

echo Step 1: Installing dotnet-ef tool...
dotnet tool update --global dotnet-ef
echo.

echo Step 2: Creating database migration...
dotnet ef migrations add InitialCreate
echo.

echo Step 3: Updating database...
dotnet ef database update
echo.

echo ====================================
echo Database setup complete!
echo ====================================
echo.
echo You can now run the application with:
echo dotnet run
echo.
pause
