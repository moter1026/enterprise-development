using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Enums;

namespace CarRentalPoint.Domain.InitialData;

/// <summary>
/// Main service class that provides initial test data for the car rental system
/// Contains predefined collections of car models, generations, cars, clients, and rental records
/// </summary>
public class CarRentalDataSeeder
{
    /// <summary>
    /// Gets the list of available car models with their specifications
    /// </summary>
    public static List<CarModel> CarModels =>
    [
        new()
        {
            Id = 1,
            Name = "Toyota Camry",
            DriveType = CarDriveType.FrontWheel,
            SeatsCount = 5,
            BodyType = CarBodyType.Sedan,
            CarClass = CarClass.D
        },
        new()
        {
            Id = 2,
            Name = "BMW X5",
            DriveType = CarDriveType.AllWheel,
            SeatsCount = 5,
            BodyType = CarBodyType.SUV,
            CarClass = CarClass.E
        },
        new()
        {
            Id = 3,
            Name = "Honda Civic",
            DriveType = CarDriveType.FrontWheel,
            SeatsCount = 5,
            BodyType = CarBodyType.Sedan,
            CarClass = CarClass.C
        },
        new()
        {
            Id = 4,
            Name = "Mercedes E-Class",
            DriveType = CarDriveType.RearWheel,
            SeatsCount = 5,
            BodyType = CarBodyType.Sedan,
            CarClass = CarClass.E
        },
        new()
        {
            Id = 5,
            Name = "Audi A4",
            DriveType = CarDriveType.AllWheel,
            SeatsCount = 5,
            BodyType = CarBodyType.Sedan,
            CarClass = CarClass.D
        }
    ];

    /// <summary>
    /// Gets the list of model generations with technical details and rental pricing
    /// </summary>
    public static List<ModelGeneration> ModelGenerations =>
    [
        new()
        {
            Id = 1,
            Year = 2020,
            EngineVolume = 2.5,
            TransmissionType = "Automatic",
            CarModelId = 1,
            RentalCostPerHour = 1500
        },
        new()
        {
            Id = 2,
            Year = 2021,
            EngineVolume = 3.0,
            TransmissionType = "Automatic",
            CarModelId = 2,
            RentalCostPerHour = 3000
        },
        new()
        {
            Id = 3,
            Year = 2019,
            EngineVolume = 1.8,
            TransmissionType = "Manual",
            CarModelId = 3,
            RentalCostPerHour = 1000
        },
        new()
        {
            Id = 4,
            Year = 2022,
            EngineVolume = 2.0,
            TransmissionType = "Automatic",
            CarModelId = 4,
            RentalCostPerHour = 2500
        },
        new()
        {
            Id = 5,
            Year = 2021,
            EngineVolume = 2.0,
            TransmissionType = "Automatic",
            CarModelId = 5,
            RentalCostPerHour = 2000
        }
    ];

    /// <summary>
    /// Gets the list of physical cars available in the rental fleet
    /// Each car has a unique license plate, color, and belongs to a specific generation
    /// </summary>
    public static List<Car> Cars =>
    [
        new()
        {
            Id = 1,
            LicensePlate = "А001АА",
            Color = "Black",
            ModelGenerationId = 1
        },
        new()
        {
            Id = 2,
            LicensePlate = "В002ВВ",
            Color = "White",
            ModelGenerationId = 1
        },
        new()
        {
            Id = 3,
            LicensePlate = "С003СС",
            Color = "Blue",
            ModelGenerationId = 2
        },
        new()
        {
            Id = 4,
            LicensePlate = "D004DD",
            Color = "Red",
            ModelGenerationId = 2
        },
        new()
        {
            Id = 5,
            LicensePlate = "Е005ЕЕ",
            Color = "Green",
            ModelGenerationId = 3
        },
        new()
        {
            Id = 6,
            LicensePlate = "F006FF",
            Color = "Gray",
            ModelGenerationId = 3
        },
        new()
        {
            Id = 7,
            LicensePlate = "G007GG",
            Color = "Black",
            ModelGenerationId = 4
        },
        new()
        {
            Id = 8,
            LicensePlate = "H008HH",
            Color = "White",
            ModelGenerationId = 4
        },
        new()
        {
            Id = 9,
            LicensePlate = "I009II",
            Color = "Blue",
            ModelGenerationId = 5
        },
        new()
        {
            Id = 10,
            LicensePlate = "J010JJ",
            Color = "Red",
            ModelGenerationId = 5
        },
        new()
        {
            Id = 11,
            LicensePlate = "K011KK",
            Color = "Green",
            ModelGenerationId = 1
        },
        new()
        {
            Id = 12,
            LicensePlate = "L012LL",
            Color = "Gray",
            ModelGenerationId = 2
        }
    ];

    /// <summary>
    /// Gets the list of registered clients who can rent vehicles
    /// Contains client identification and personal information
    /// </summary>
    public static List<Client> Clients =>
    [
        new()
        {
            Id = 1,
            DriverLicenseNumber = "1234567890",
            FullName = "Ivanov Ivan Ivanovich",
            BirthDate = new DateOnly(1985, 5, 15)
        },
        new()
        {
            Id = 2,
            DriverLicenseNumber = "2345678901",
            FullName = "Petrov Petr Petrovich",
            BirthDate = new DateOnly(1990, 8, 22)
        },
        new()
        {
            Id = 3,
            DriverLicenseNumber = "3456789012",
            FullName = "Sidorov Alexey Vladimirovich",
            BirthDate = new DateOnly(1988, 3, 10)
        },
        new()
        {
            Id = 4,
            DriverLicenseNumber = "4567890123",
            FullName = "Kuznetsova Maria Sergeevna",
            BirthDate = new DateOnly(1992, 11, 5)
        },
        new()
        {
            Id = 5,
            DriverLicenseNumber = "5678901234",
            FullName = "Smirnov Dmitry Alexeevich",
            BirthDate = new DateOnly(1987, 7, 18)
        },
        new()
        {
            Id = 6,
            DriverLicenseNumber = "6789012345",
            FullName = "Popova Ekaterina Andreevna",
            BirthDate = new DateOnly(1995, 2, 28)
        },
        new()
        {
            Id = 7,
            DriverLicenseNumber = "7890123456",
            FullName = "Vasilyev Andrey Nikolaevich",
            BirthDate = new DateOnly(1983, 9, 12)
        },
        new()
        {
            Id = 8,
            DriverLicenseNumber = "8901234567",
            FullName = "Novikova Olga Viktorovna",
            BirthDate = new DateOnly(1991, 6, 8)
        },
        new()
        {
            Id = 9,
            DriverLicenseNumber = "9012345678",
            FullName = "Morozov Sergey Ivanovich",
            BirthDate = new DateOnly(1989, 4, 25)
        },
        new()
        {
            Id = 10,
            DriverLicenseNumber = "0123456789",
            FullName = "Volkova Anna Dmitrievna",
            BirthDate = new DateOnly(1993, 12, 3)
        },
        new()
        {
            Id = 11,
            DriverLicenseNumber = "1122334455",
            FullName = "Alexeev Pavel Olegovich",
            BirthDate = new DateOnly(1986, 1, 20)
        },
        new()
        {
            Id = 12,
            DriverLicenseNumber = "2233445566",
            FullName = "Nikitina Irina Sergeevna",
            BirthDate = new DateOnly(1994, 10, 15)
        }
    ];


    /// <summary>
    /// Gets the list of rental records representing completed and active rental agreements
    /// Each rental links a client with a car for a specific time period
    /// </summary>
    public static List<Rental> Rentals =>
    [
        new()
        {
            Id = 1,
            ClientId = 1,
            CarId = 1,
            RentalStart = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 2,
            ClientId = 2,
            CarId = 2,
            RentalStart = new DateTime(2024, 1, 16, 14, 30, 0, DateTimeKind.Utc),
            RentalHours = 48
        },
        new()
        {
            Id = 3,
            ClientId = 3,
            CarId = 3,
            RentalStart = new DateTime(2024, 1, 17, 9, 15, 0, DateTimeKind.Utc),
            RentalHours = 12
        },
        new()
        {
            Id = 4,
            ClientId = 4,
            CarId = 4,
            RentalStart = new DateTime(2024, 1, 14, 14, 0, 0, DateTimeKind.Utc),
            RentalHours = 72
        },
        new()
        {
            Id = 5,
            ClientId = 5,
            CarId = 5,
            RentalStart = new DateTime(2024, 1, 19, 11, 0, 0, DateTimeKind.Utc),
            RentalHours = 36
        },
        new()
        {
            Id = 6,
            ClientId = 6,
            CarId = 6,
            RentalStart = new DateTime(2024, 1, 20, 13, 20, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 7,
            ClientId = 6,
            CarId = 6,
            RentalStart = new DateTime(2024, 1, 22, 13, 20, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 8,
            ClientId = 6,
            CarId = 6,
            RentalStart = new DateTime(2024, 1, 25, 13, 20, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 9,
            ClientId = 7,
            CarId = 7,
            RentalStart = new DateTime(2024, 1, 21, 8, 0, 0, DateTimeKind.Utc),
            RentalHours = 60
        },
        new()
        {
            Id = 10,
            ClientId = 8,
            CarId = 8,
            RentalStart = new DateTime(2024, 1, 15, 15, 30, 0, DateTimeKind.Utc),
            RentalHours = 18
        },
        new()
        {
            Id = 11,
            ClientId = 8,
            CarId = 8,
            RentalStart = new DateTime(2024, 2, 15, 15, 30, 0, DateTimeKind.Utc),
            RentalHours = 18
        },
        new()
        {
            Id = 12,
            ClientId = 9,
            CarId = 9,
            RentalStart = new DateTime(2024, 1, 16, 12, 0, 0, DateTimeKind.Utc),
            RentalHours = 42
        },
        new()
        {
            Id = 13,
            ClientId = 10,
            CarId = 10,
            RentalStart = new DateTime(2024, 1, 17, 17, 45, 0   , DateTimeKind.Utc),
            RentalHours = 30
        },
        new()
        {
            Id = 14,
            ClientId = 11,
            CarId = 11,
            RentalStart = new DateTime(2024, 1, 18, 10, 15, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 15,
            ClientId = 11,
            CarId = 11,
            RentalStart = new DateTime(2024, 1, 17, 10, 15, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 16,
            ClientId = 12,
            CarId = 12,
            RentalStart = new DateTime(2024, 1, 19, 14, 0, 0, DateTimeKind.Utc),
            RentalHours = 54
        },
        new()
        {
            Id = 17,
            ClientId = 1,
            CarId = 1,
            RentalStart = new DateTime(2024, 1, 15, 15, 0, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 18,
            ClientId = 1,
            CarId = 1,
            RentalStart = new DateTime(2024, 2, 15, 10, 0, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 19,
            ClientId = 1,
            CarId = 1,
            RentalStart = new DateTime(2024, 1, 16, 10, 0, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 20,
            ClientId = 1,
            CarId = 1,
            RentalStart = new DateTime(2024, 5, 15, 10, 0, 0, DateTimeKind.Utc),
            RentalHours = 24
        },
        new()
        {
            Id = 21,
            ClientId = 4,
            CarId = 4,
            RentalStart = new DateTime(2024, 1, 15, 14, 0, 0, DateTimeKind.Utc),
            RentalHours = 72
        },
        new()
        {
            Id = 22,
            ClientId = 4,
            CarId = 4,
            RentalStart = new DateTime(2024, 2, 14, 14, 0, 0, DateTimeKind.Utc),
            RentalHours = 72
        }
    ];
}