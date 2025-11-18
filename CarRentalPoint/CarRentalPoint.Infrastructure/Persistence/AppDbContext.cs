using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for the car rental application.
/// </summary>
/// <param name="options">The options for configuring the database context.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// DbSet of cars available for rental.
    /// </summary>
    public DbSet<Car> Cars => Set<Car>();

    /// <summary>
    /// DbSet of car models.
    /// </summary>
    public DbSet<CarModel> CarModels => Set<CarModel>();

    /// <summary>
    /// DbSet of clients who rent cars.
    /// </summary>
    public DbSet<Client> Clients => Set<Client>();

    /// <summary>
    /// DbSet of rental records.
    /// </summary>
    public DbSet<Rental> Rentals => Set<Rental>();

    /// <summary>
    /// DbSet of car model generations.
    /// </summary>
    public DbSet<ModelGeneration> Generations => Set<ModelGeneration>();

    /// <summary>
    /// Configures the EF Core model and entity relationships.
    /// </summary>
    /// <param name="modelBuilder">Model builder used to configure entity models.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.HasKey(cm => cm.Id);
            entity.Property(cm => cm.Name).IsRequired();
            entity.Property(cm => cm.DriveType).IsRequired();
            entity.Property(cm => cm.SeatsCount).IsRequired();
            entity.Property(cm => cm.BodyType).IsRequired();
            entity.Property(cm => cm.CarClass).IsRequired();
        });

        modelBuilder.Entity<ModelGeneration>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Year).IsRequired();
            entity.Property(g => g.EngineVolume).IsRequired();
            entity.Property(g => g.TransmissionType).IsRequired();
            entity.Property(g => g.RentalCostPerHour).IsRequired();

            entity.HasOne(g => g.Model)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.LicensePlate).IsRequired();
            entity.Property(c => c.Color).IsRequired();

            entity.HasOne(c => c.Generation)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.FullName).IsRequired();
            entity.Property(c => c.DriverLicenseNumber).IsRequired();
            entity.Property(c => c.BirthDate).IsRequired();
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.RentalStart).IsRequired();
            entity.Property(r => r.RentalHours).IsRequired();

            entity.HasOne(r => r.Client)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Car)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
