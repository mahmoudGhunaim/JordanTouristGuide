using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JordanTouristGuide.Models;

namespace JordanTouristGuide.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<ExperienceDb> Experiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .Property(b => b.BookingDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Contact>()
                .Property(c => c.SubmittedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Property>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ExperienceDb>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany()
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ExperienceDb>()
                .HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Property>()
                .Property(p => p.PricePerNight)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ExperienceDb>()
                .Property(e => e.Price)
                .HasPrecision(18, 2);
        }
    }
}
