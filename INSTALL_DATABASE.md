# Database Installation Guide

## The Problem
You're getting this error because SQL Server LocalDB is not installed on your system.

## Solution Options

### Option 1: Install SQL Server Express (Recommended)

**Download and Install SQL Server Express 2022:**
1. Visit: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. Click "Download now" under "Express" edition (free)
3. Run the installer
4. Choose "Basic" installation
5. Accept the license terms
6. Wait for installation to complete
7. Note the instance name (usually `SQLEXPRESS` or `MSSQLSERVER`)

**After Installation:**
Update your connection string in `appsettings.json` to:
```json
"Server=.\\SQLEXPRESS;Database=JordanTouristGuide;Trusted_Connection=True;TrustServerCertificate=True"
```

---

### Option 2: Install SQL Server LocalDB Only (Lightweight)

**Download:**
https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb

**Or use this direct link:**
https://download.microsoft.com/download/3/8/d/38de7036-2433-4207-8eae-06e247e17b25/SqlLocalDB.msi

**Install:**
1. Run SqlLocalDB.msi
2. Follow the installation wizard
3. After installation, restart your computer

**After Installation:**
Your current connection string should work. Just run:
```bash
dotnet ef database update
```

---

### Option 3: Use SQLite (No Installation Required)

This is the EASIEST option - no SQL Server installation needed!

I can modify your project to use SQLite instead, which is a file-based database that requires no server installation.

Would you like me to convert the project to SQLite?

---

### Option 4: Use Existing SQL Server (If You Have One)

If you already have SQL Server installed, check your instance name:

**Check installed SQL Server instances:**
```bash
# Open Command Prompt as Administrator
sqlcmd -L
```

Or check in:
- Services (Win+R → services.msc)
- Look for "SQL Server (INSTANCENAME)"

**Common connection strings:**

For default instance:
```json
"Server=localhost;Database=JordanTouristGuide;Trusted_Connection=True;TrustServerCertificate=True"
```

For named instance:
```json
"Server=.\\SQLEXPRESS;Database=JordanTouristGuide;Trusted_Connection=True;TrustServerCertificate=True"
```

For remote server:
```json
"Server=YOUR_SERVER_NAME;Database=JordanTouristGuide;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True"
```

---

## Quick Fix - I Recommend Option 3 (SQLite)

Since SQLite requires NO installation and works immediately, would you like me to:
1. Convert the project to use SQLite
2. Update all necessary files
3. You can run it immediately without any database server

Just say "Yes, use SQLite" and I'll make the changes!

---

## After Fixing the Connection

Once you choose an option and the database connection works:

1. Run migrations:
   ```bash
   dotnet ef database update
   ```

2. Start the application:
   ```bash
   dotnet run
   ```

3. Open browser:
   ```
   https://localhost:5001
   ```

---

## Need Help?

If you're unsure which option to choose:
- **Quickest/Easiest**: Option 3 (SQLite) - I can set this up in 2 minutes
- **Most Professional**: Option 1 (SQL Server Express) - Takes 10-15 minutes to install
- **Already Have SQL Server**: Option 4 - Just need to find your instance name

Let me know which option you prefer!
