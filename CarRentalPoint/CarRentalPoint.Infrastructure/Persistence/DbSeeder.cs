using CarRentalPoint.Domain.InitialData;
using Microsoft.EntityFrameworkCore;
namespace CarRentalPoint.Infrastructure.Persistence;

/// <summary>
/// Provides methods for seeding initial data into the database.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Seeds car models into the database if none exist.
    /// </summary>
    /// <param name="context">Database context.</param>
    private static async Task SeedCarModelsAsync(AppDbContext context)
    {
        if (!context.CarModels.Any())
        {
            await context.CarModels.AddRangeAsync(CarRentalDataSeeder.CarModels);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SELECT setval('car_model_id_seq', (SELECT MAX(id) FROM car_model))");
        }
    }

    /// <summary>
    /// Seeds model generations and links them with existing car models.
    /// </summary>
    /// <param name="context">Database context.</param>
    private static async Task SeedModelGenerationsAsync(AppDbContext context)
    {
        if (!context.Generations.Any())
        {
            await context.Generations.AddRangeAsync(CarRentalDataSeeder.ModelGenerations);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SELECT setval('model_generation_id_seq', (SELECT MAX(id) FROM model_generation))");
        }
    }

    /// <summary>
    /// Seeds cars and assigns them to the corresponding model generations.
    /// </summary>
    /// <param name="context">Database context.</param>
    private static async Task SeedCarsAsync(AppDbContext context)
    {
        if (!context.Cars.Any())
        {
            await context.Cars.AddRangeAsync(CarRentalDataSeeder.Cars);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SELECT setval('car_id_seq', (SELECT MAX(id) FROM car))");
        }
    }

    /// <summary>
    /// Seeds clients into the database.
    /// Ensures birth dates are stored as UTC.
    /// </summary>
    /// <param name="context">Database context.</param>
    private static async Task SeedClientsAsync(AppDbContext context)
    {
        if (!context.Clients.Any())
        {
            await context.Clients.AddRangeAsync(CarRentalDataSeeder.Clients);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SELECT setval('client_id_seq', (SELECT MAX(id) FROM client))");
        }
    }

    /// <summary>
    /// Seeds rental records, linking them with existing cars and clients.
    /// </summary>
    /// <param name="context">Database context.</param>
    private static async Task SeedRentalsAsync(AppDbContext context)
    {
        if (!context.Rentals.Any())
        {
            await context.Rentals.AddRangeAsync(CarRentalDataSeeder.Rentals);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SELECT setval('rental_id_seq', (SELECT MAX(id) FROM rental))");
        }
    }

    /// <summary>
    /// Seeds all database tables in the correct order.
    /// </summary>
    /// <param name="context">Database context.</param>
    public static async Task SeedAllAsync(AppDbContext context)
    {
        await SeedCarModelsAsync(context);
        await SeedModelGenerationsAsync(context);
        await SeedCarsAsync(context);
        await SeedClientsAsync(context);
        await SeedRentalsAsync(context);
    }
}
