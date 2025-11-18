using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
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
    public static async Task SeedCarModelsAsync(AppDbContext context)
    {
        if (!context.CarModels.Any())
        {
            context.CarModels.AddRange(CarRentalDataSeeder.CarModels);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""CarModels""', 'Id'), (SELECT MAX(""Id"") FROM ""CarModels""));"
        );
    }

    /// <summary>
    /// Seeds model generations and links them with existing car models.
    /// </summary>
    /// <param name="context">Database context.</param>
    public static async Task SeedModelGenerationsAsync(AppDbContext context)
    {
        if (!context.Generations.Any())
        {
            var dbCarModels = await context.CarModels.ToListAsync();
            var generationsToAdd = CarRentalDataSeeder.ModelGenerations
                .Select(gen => new ModelGeneration
                {
                    Year = gen.Year,
                    EngineVolume = gen.EngineVolume,
                    TransmissionType = gen.TransmissionType,
                    RentalCostPerHour = gen.RentalCostPerHour,
                    Model = dbCarModels.First(m => m.Name == gen.Model.Name)
                })
                .ToList();

            context.Generations.AddRange(generationsToAdd);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""Generations""', 'Id'), (SELECT MAX(""Id"") FROM ""Generations""));"
        );
    }

    /// <summary>
    /// Seeds cars and assigns them to the corresponding model generations.
    /// </summary>
    /// <param name="context">Database context.</param>
    public static async Task SeedCarsAsync(AppDbContext context)
    {
        if (!context.Cars.Any())
        {
            var dbGenerations = context.Generations.Include(g => g.Model).ToList();

            var carsToAdd = new CarRentalDataSeeder().Cars
                .Select(car =>
                {
                    var generation = dbGenerations.First(g =>
                        g.Year == car.Generation.Year &&
                        g.EngineVolume == car.Generation.EngineVolume &&
                        g.TransmissionType == car.Generation.TransmissionType &&
                        g.Model.Name == car.Generation.Model.Name
                    );

                    return new Car
                    {
                        LicensePlate = car.LicensePlate,
                        Color = car.Color,
                        Generation = generation
                    };
                })
                .ToList();

            context.Cars.AddRange(carsToAdd);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""Cars""', 'Id'), (SELECT MAX(""Id"") FROM ""Cars""));"
        );
    }

    /// <summary>
    /// Seeds clients into the database.
    /// Ensures birth dates are stored as UTC.
    /// </summary>
    /// <param name="context">Database context.</param>
    public static async Task SeedClientsAsync(AppDbContext context)
    {
        if (!context.Clients.Any())
        {
            var clients = new CarRentalDataSeeder().Clients
                .Select(c => new Client
                {
                    DriverLicenseNumber = c.DriverLicenseNumber,
                    FullName = c.FullName,
                    BirthDate = DateTime.SpecifyKind(c.BirthDate, DateTimeKind.Utc)
                })
                .ToList();

            context.Clients.AddRange(clients);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""Clients""', 'Id'), (SELECT MAX(""Id"") FROM ""Clients""));"
        );
    }

    /// <summary>
    /// Seeds rental records, linking them with existing cars and clients.
    /// </summary>
    /// <param name="context">Database context.</param>
    public static async Task SeedRentalsAsync(AppDbContext context)
    {
        if (!context.Rentals.Any())
        {
            var dbCars = context.Cars.ToList();
            var dbClients = context.Clients.ToList();

            var rentalsToAdd = new CarRentalDataSeeder().Rentals
                .Select(r => new Rental
                {
                    RentalStart = DateTime.SpecifyKind(r.RentalStart, DateTimeKind.Utc),
                    RentalHours = r.RentalHours,
                    Car = dbCars.First(c => c.Id == r.Car.Id),
                    Client = dbClients.First(c => c.Id == r.Client.Id)
                })
                .ToList();

            context.Rentals.AddRange(rentalsToAdd);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""Rentals""', 'Id'), (SELECT MAX(""Id"") FROM ""Rentals""));"
        );
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
