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
            entity.ToTable("car_model");
            entity.HasKey(cm => cm.Id);
            entity.Property(cm => cm.Id).HasColumnName("id");
            entity.Property(cm => cm.Name).HasColumnName("name").IsRequired();
            entity.Property(cm => cm.DriveType).HasColumnName("drive_type").IsRequired();
            entity.Property(cm => cm.SeatsCount).HasColumnName("seats_count").IsRequired();
            entity.Property(cm => cm.BodyType).HasColumnName("body_type").IsRequired();
            entity.Property(cm => cm.CarClass).HasColumnName("car_class").IsRequired();
        });

        modelBuilder.Entity<ModelGeneration>(entity =>
        {
            entity.ToTable("model_generation");
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id).HasColumnName("id");
            entity.Property(g => g.Year).HasColumnName("year").IsRequired();
            entity.Property(g => g.EngineVolume).HasColumnName("engine_volume").IsRequired();
            entity.Property(g => g.TransmissionType).HasColumnName("transmission_type").IsRequired();
            entity.Property(g => g.RentalCostPerHour).HasColumnName("rental_cost_per_hour").IsRequired();
            entity.Property(g => g.CarModelId).HasColumnName("car_model_id").IsRequired();
            entity.HasOne(g => g.Model)
                  .WithMany()
                  .HasForeignKey(g => g.CarModelId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.ToTable("car");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.LicensePlate).HasColumnName("license_plate").IsRequired();
            entity.Property(c => c.Color).HasColumnName("color").IsRequired();
            entity.Property(c => c.ModelGenerationId).HasColumnName("model_generation_id").IsRequired();
            entity.HasOne(c => c.Generation)
                  .WithMany()
                  .HasForeignKey(c => c.ModelGenerationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("client");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.FullName).HasColumnName("full_name").IsRequired();
            entity.Property(c => c.DriverLicenseNumber).HasColumnName("driver_license_number").IsRequired();
            entity.Property(c => c.BirthDate).HasColumnName("birth_date").IsRequired();
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.ToTable("rental");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasColumnName("id");
            entity.Property(r => r.ClientId).HasColumnName("client_id").IsRequired();
            entity.Property(r => r.CarId).HasColumnName("car_id").IsRequired();
            entity.Property(r => r.RentalStart).HasColumnName("rental_start").IsRequired();
            entity.Property(r => r.RentalHours).HasColumnName("rental_hours").IsRequired();
            entity.HasOne(r => r.Client)
                  .WithMany()
                  .HasForeignKey(r => r.ClientId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(r => r.Car)
                  .WithMany()
                  .HasForeignKey(r => r.CarId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
