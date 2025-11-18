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
            Year = 2020,
            EngineVolume = 2.5,
            TransmissionType = "Automatic",
            Model = CarModels[0],
            RentalCostPerHour = 1500
        },
        new()
        {
            Year = 2021,
            EngineVolume = 3.0,
            TransmissionType = "Automatic",
            Model = CarModels[1],
            RentalCostPerHour = 3000
        },
        new()
        {
            Year = 2019,
            EngineVolume = 1.8,
            TransmissionType = "Manual",
            Model = CarModels[2],
            RentalCostPerHour = 1000
        },
        new()
        {
            Year = 2022,
            EngineVolume = 2.0,
            TransmissionType = "Automatic",
            Model = CarModels[3],
            RentalCostPerHour = 2500
        },
        new()
        {
            Year = 2021,
            EngineVolume = 2.0,
            TransmissionType = "Automatic",
            Model = CarModels[4],
            RentalCostPerHour = 2000
        }
    ];

    /// <summary>
    /// Gets the list of physical cars available in the rental fleet
    /// Each car has a unique license plate, color, and belongs to a specific generation
    /// </summary>
    public List<Car> Cars =>
    [
        new()
        {
            Id = 1,
            LicensePlate = "А001АА",
            Color = "Black",
            Generation = ModelGenerations[0]
        },
        new()
        {
            Id = 2,
            LicensePlate = "В002ВВ",
            Color = "White",
            Generation = ModelGenerations[0]
        },
        new()
        {
            Id = 3,
            LicensePlate = "С003СС",
            Color = "Blue",
            Generation = ModelGenerations[1]
        },
        new()
        {
            Id = 4,
            LicensePlate = "D004DD",
            Color = "Red",
            Generation = ModelGenerations[1]
        },
        new()
        {
            Id = 5,
            LicensePlate = "Е005ЕЕ",
            Color = "Green",
            Generation = ModelGenerations[2]
        },
        new()
        {
            Id = 6,
            LicensePlate = "F006FF",
            Color = "Gray",
            Generation = ModelGenerations[2]
        },
        new()
        {
            Id = 7,
            LicensePlate = "G007GG",
            Color = "Black",
            Generation = ModelGenerations[3]
        },
        new()
        {
            Id = 8,
            LicensePlate = "H008HH",
            Color = "White",
            Generation = ModelGenerations[3]
        },
        new()
        {
            Id = 9,
            LicensePlate = "I009II",
            Color = "Blue",
            Generation = ModelGenerations[4]
        },
        new()
        {
            Id = 10,
            LicensePlate = "J010JJ",
            Color = "Red",
            Generation = ModelGenerations[4]
        },
        new()
        {
            Id = 11,
            LicensePlate = "K011KK",
            Color = "Green",
            Generation = ModelGenerations[0]
        },
        new()
        {
            Id = 12,
            LicensePlate = "L012LL",
            Color = "Gray",
            Generation = ModelGenerations[1]
        }
    ];

    /// <summary>
    /// Gets the list of registered clients who can rent vehicles
    /// Contains client identification and personal information
    /// </summary>
    public List<Client> Clients =>
    [
        new()
        {
            Id = 1,
            DriverLicenseNumber = "1234567890",
            FullName = "Ivanov Ivan Ivanovich",
            BirthDate = new DateTime(1985, 5, 15)
        },
        new()
        {
            Id = 2,
            DriverLicenseNumber = "2345678901",
            FullName = "Petrov Petr Petrovich",
            BirthDate = new DateTime(1990, 8, 22)
        },
        new()
        {
            Id = 3,
            DriverLicenseNumber = "3456789012",
            FullName = "Sidorov Alexey Vladimirovich",
            BirthDate = new DateTime(1988, 3, 10)
        },
        new()
        {
            Id = 4,
            DriverLicenseNumber = "4567890123",
            FullName = "Kuznetsova Maria Sergeevna",
            BirthDate = new DateTime(1992, 11, 5)
        },
        new()
        {
            Id = 5,
            DriverLicenseNumber = "5678901234",
            FullName = "Smirnov Dmitry Alexeevich",
            BirthDate = new DateTime(1987, 7, 18)
        },
        new()
        {
            Id = 6,
            DriverLicenseNumber = "6789012345",
            FullName = "Popova Ekaterina Andreevna",
            BirthDate = new DateTime(1995, 2, 28)
        },
        new()
        {
            Id = 7,
            DriverLicenseNumber = "7890123456",
            FullName = "Vasilyev Andrey Nikolaevich",
            BirthDate = new DateTime(1983, 9, 12)
        },
        new()
        {
            Id = 8,
            DriverLicenseNumber = "8901234567",
            FullName = "Novikova Olga Viktorovna",
            BirthDate = new DateTime(1991, 6, 8)
        },
        new()
        {
            Id = 9,
            DriverLicenseNumber = "9012345678",
            FullName = "Morozov Sergey Ivanovich",
            BirthDate = new DateTime(1989, 4, 25)
        },
        new()
        {
            Id = 10,
            DriverLicenseNumber = "0123456789",
            FullName = "Volkova Anna Dmitrievna",
            BirthDate = new DateTime(1993, 12, 3)
        },
        new()
        {
            Id = 11,
            DriverLicenseNumber = "1122334455",
            FullName = "Alexeev Pavel Olegovich",
            BirthDate = new DateTime(1986, 1, 20)
        },
        new()
        {
            Id = 12,
            DriverLicenseNumber = "2233445566",
            FullName = "Nikitina Irina Sergeevna",
            BirthDate = new DateTime(1994, 10, 15)
        }
    ];

    /// <summary>
    /// Gets the list of rental records representing completed and active rental agreements
    /// Each rental links a client with a car for a specific time period
    /// </summary>
    public List<Rental> Rentals =>
    [
        new()
        {
            Id = 1,
            Client = Clients[0],
            Car = Cars[0],
            RentalStart = new DateTime(2024, 1, 15, 10, 0, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 2,
            Client = Clients[1],
            Car = Cars[1],
            RentalStart = new DateTime(2024, 1, 16, 14, 30, 0),
            RentalHours = 48
        },
        new()
        {
            Id = 3,
            Client = Clients[2],
            Car = Cars[2],
            RentalStart = new DateTime(2024, 1, 17, 9, 15, 0),
            RentalHours = 12
        },
        new()
        {
            Id = 4,
            Client = Clients[3],
            Car = Cars[3],
            RentalStart = new DateTime(2024, 1, 14, 14, 0, 0),
            RentalHours = 72
        },
        new()
        {
            Id = 5,
            Client = Clients[4],
            Car = Cars[4],
            RentalStart = new DateTime(2024, 1, 19, 11, 0, 0),
            RentalHours = 36
        },
        new()
        {
            Id = 6,
            Client = Clients[5],
            Car = Cars[5],
            RentalStart = new DateTime(2024, 1, 20, 13, 20, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 7,
            Client = Clients[5],
            Car = Cars[5],
            RentalStart = new DateTime(2024, 1, 22, 13, 20, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 8,
            Client = Clients[5],
            Car = Cars[5],
            RentalStart = new DateTime(2024, 1, 25, 13, 20, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 9,
            Client = Clients[6],
            Car = Cars[6],
            RentalStart = new DateTime(2024, 1, 21, 8, 0, 0),
            RentalHours = 60
        },
        new()
        {
            Id = 10,
            Client = Clients[7],
            Car = Cars[7],
            RentalStart = new DateTime(2024, 1, 15, 15, 30, 0),
            RentalHours = 18
        },
        new()
        {
            Id = 11,
            Client = Clients[7],
            Car = Cars[7],
            RentalStart = new DateTime(2024, 2, 15, 15, 30, 0),
            RentalHours = 18
        },
        new()
        {
            Id = 12,
            Client = Clients[8],
            Car = Cars[8],
            RentalStart = new DateTime(2024, 1, 16, 12, 0, 0),
            RentalHours = 42
        },
        new()
        {
            Id = 13,
            Client = Clients[9],
            Car = Cars[9],
            RentalStart = new DateTime(2024, 1, 17, 17, 45, 0),
            RentalHours = 30
        },
        new()
        {
            Id = 14,
            Client = Clients[10],
            Car = Cars[10],
            RentalStart = new DateTime(2024, 1, 18, 10, 15, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 15,
            Client = Clients[10],
            Car = Cars[10],
            RentalStart = new DateTime(2024, 1, 17, 10, 15, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 16,
            Client = Clients[11],
            Car = Cars[11],
            RentalStart = new DateTime(2024, 1, 19, 14, 0, 0),
            RentalHours = 54
        },
        new()
        {
            Id = 17,
            Client = Clients[0],
            Car = Cars[0],
            RentalStart = new DateTime(2024, 1, 15, 15, 0, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 18,
            Client = Clients[0],
            Car = Cars[0],
            RentalStart = new DateTime(2024, 2, 15, 10, 0, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 19,
            Client = Clients[0],
            Car = Cars[0],
            RentalStart = new DateTime(2024, 1, 16, 10, 0, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 20,
            Client = Clients[0],
            Car = Cars[0],
            RentalStart = new DateTime(2024, 5, 15, 10, 0, 0),
            RentalHours = 24
        },
        new()
        {
            Id = 21,
            Client = Clients[3],
            Car = Cars[3],
            RentalStart = new DateTime(2024, 1, 15, 14, 0, 0),
            RentalHours = 72
        },
        new()
        {
            Id = 22,
            Client = Clients[3],
            Car = Cars[3],
            RentalStart = new DateTime(2024, 2, 14, 14, 0, 0),
            RentalHours = 72
        },
    ];
}