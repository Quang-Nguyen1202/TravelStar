using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TravelStar.Entities;

public class TravelStarContext : IdentityDbContext<AppUser>
{
    public TravelStarContext(DbContextOptions<TravelStarContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking>? Booking { get; set; }
    public virtual DbSet<City>? City { get; set; }
    public virtual DbSet<Customer>? Customer { get; set; }
    public virtual DbSet<District>? District { get; set; }
    public virtual DbSet<Hotel>? Hotel { get; set; }
    public virtual DbSet<Room>? Room { get; set; }
    public virtual DbSet<Ward>? Ward { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Booking>(e =>
        {
            e.HasKey(p => p.Id);

            e.Property(p => p.Id).ValueGeneratedOnAdd();

            e.HasOne(p => p.Customer)
             .WithMany(u => u.Bookings)
             .HasForeignKey(p => p.CustomerId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Customer>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).ValueGeneratedOnAdd();
        });

        builder.Entity<Room>(e =>
        {
            e.HasKey(p => p.Id);

            e.Property(p => p.Id).ValueGeneratedOnAdd();

            e.HasOne(p => p.Hotel)
             .WithMany(u => u.Rooms)
             .HasForeignKey(p => p.HotelId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Hotel>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).ValueGeneratedOnAdd();

            e.HasOne(p => p.Ward)
             .WithMany(u => u.Hotels)
             .HasForeignKey(p => p.WardId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(h => h.User)
             .WithMany(u => u.Hotels)
             .HasForeignKey(h => h.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Ward>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).ValueGeneratedOnAdd();

            e.HasOne(p => p.District)
             .WithMany(u => u.Wards)
             .HasForeignKey(p => p.DistrictId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<District>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).ValueGeneratedOnAdd();

            e.HasOne(p => p.City)
             .WithMany(u => u.Districts)
             .HasForeignKey(p => p.CityId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<City>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).ValueGeneratedOnAdd();
        });
    }
}
