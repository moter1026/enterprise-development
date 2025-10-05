using CarRentalPoint.Contract;
using CarRentalPoint.Entities;

namespace CarRentalPoint.InitialData;


// Основной класс сервиса
public class CarRentalService
{
    protected static List<CarModel> CarModels =>
        [
            new () 
            { 
                Name = "Toyota Camry",
                DriveType = "Передний",
                SeatsCount = 5, 
                BodyType = "Седан",
                CarClass = "D" 
            },
            new () 
            { 
                Name = "BMW X5",
                DriveType = "Полный",
                SeatsCount = 5,
                BodyType = "Внедорожник",
                CarClass = "E" 
            },
            new() 
            { 
                Name = "Honda Civic", 
                DriveType = "Передний",
                SeatsCount = 5, 
                BodyType = "Седан",
                CarClass = "C" 
            },
            new() 
            {
                Name = "Mercedes E-Class", 
                DriveType = "Задний",
                SeatsCount = 5,
                BodyType = "Седан",
                CarClass = "E" 
            },
            new() 
            {
                Name = "Audi A4",
                DriveType = "Полный",
                SeatsCount = 5,
                BodyType = "Седан",
                CarClass = "D" 
            }
        ];
    protected static List<ModelGeneration> ModelGenerations =>
        [
            new ()
            {
                Year = 2020,
                EngineVolume = 2.5,
                TransmissionType = "Автомат",
                Model = CarModels[0],
                RentalCostPerHour = 1500
            },
            new ()
            {
                Year = 2021,
                EngineVolume = 3.0,
                TransmissionType = "Автомат",
                Model = CarModels[1],
                RentalCostPerHour = 3000
            },
            new()
            {
                Year = 2019,
                EngineVolume = 1.8,
                TransmissionType = "Механика",
                Model = CarModels[2],
                RentalCostPerHour = 1000
            },
            new()
            {
                Year = 2022,
                EngineVolume = 2.0,
                TransmissionType = "Автомат",
                Model = CarModels[3],
                RentalCostPerHour = 2500
            },
            new()
            {
                Year = 2021,
                EngineVolume = 2.0,
                TransmissionType = "Автомат",
                Model = CarModels[4],
                RentalCostPerHour = 2000
            }
        ];
    public List<Car> Cars =>
        [
            new()
            {
                LicensePlate = "А001АА",
                Color = "Черный",
                Generation = ModelGenerations[0]
            },
            new()
            {
                LicensePlate = "В002ВВ",
                Color = "Белый",
                Generation = ModelGenerations[0]
            },
            new()
            {
                LicensePlate = "С003СС",
                Color = "Синий",
                Generation = ModelGenerations[1]
            },
            new()
            {
                LicensePlate = "D004DD",
                Color = "Красный",
                Generation = ModelGenerations[1]
            },
            new()
            {
                LicensePlate = "Е005ЕЕ",
                Color = "Зеленый",
                Generation = ModelGenerations[2]
            },
            new()
            {
                LicensePlate = "F006FF",
                Color = "Серый",
                Generation = ModelGenerations[2]
            },
            new()
            {
                LicensePlate = "G007GG",
                Color = "Черный",
                Generation = ModelGenerations[3]
            },
            new()
            {
                LicensePlate = "H008HH",
                Color = "Белый",
                Generation = ModelGenerations[3]
            },
            new()
            {
                LicensePlate = "I009II",
                Color = "Синий",
                Generation = ModelGenerations[4]
            },
            new()
            {
                LicensePlate = "J010JJ",
                Color = "Красный",
                Generation = ModelGenerations[4]
            },
            new()
            {
                LicensePlate = "K011KK",
                Color = "Зеленый",
                Generation = ModelGenerations[0]
            },
            new()
            {
                LicensePlate = "L012LL",
                Color = "Серый",
                Generation = ModelGenerations[1]
            }
        ];
    public List<Client> Clients =>
        [
            new()
            {
                DriverLicenseNumber = "1234567890",
                FullName = "Иванов Иван Иванович",
                BirthDate = new DateTime(1985, 5, 15)
            },
            new()
            {
                DriverLicenseNumber = "2345678901",
                FullName = "Петров Петр Петрович",
                BirthDate = new DateTime(1990, 8, 22)
            },
            new()
            {
                DriverLicenseNumber = "3456789012",
                FullName = "Сидоров Алексей Владимирович",
                BirthDate = new DateTime(1988, 3, 10)
            },
            new()
            {
                DriverLicenseNumber = "4567890123",
                FullName = "Кузнецова Мария Сергеевна",
                BirthDate = new DateTime(1992, 11, 5)
            },
            new()
            {
                DriverLicenseNumber = "5678901234",
                FullName = "Смирнов Дмитрий Алексеевич",
                BirthDate = new DateTime(1987, 7, 18)
            },
            new()
            {
                DriverLicenseNumber = "6789012345",
                FullName = "Попова Екатерина Андреевна",
                BirthDate = new DateTime(1995, 2, 28)
            },
            new()
            {
                DriverLicenseNumber = "7890123456",
                FullName = "Васильев Андрей Николаевич",
                BirthDate = new DateTime(1983, 9, 12)
            },
            new()
            {
                DriverLicenseNumber = "8901234567",
                FullName = "Новикова Ольга Викторовна",
                BirthDate = new DateTime(1991, 6, 8)
            },
            new()
            {
                DriverLicenseNumber = "9012345678",
                FullName = "Морозов Сергей Иванович",
                BirthDate = new DateTime(1989, 4, 25)
            },
            new()
            {
                DriverLicenseNumber = "0123456789",
                FullName = "Волкова Анна Дмитриевна",
                BirthDate = new DateTime(1993, 12, 3)
            },
            new()
            {
                DriverLicenseNumber = "1122334455",
                FullName = "Алексеев Павел Олегович",
                BirthDate = new DateTime(1986, 1, 20)
            },
            new()
            {
                DriverLicenseNumber = "2233445566",
                FullName = "Никитина Ирина Сергеевна",
                BirthDate = new DateTime(1994, 10, 15)
            }
        ];
    public List<Rental> Rentals =>
    [
        new() //
        {
            Client = Clients[0],
            Car = Cars[0],
            RentalStart = new DateTime(2024, 1, 15, 10, 0, 0),
            RentalHours = 24
        },
        new() //
        {
            Client = Clients[1],
            Car = Cars[1],
            RentalStart = new DateTime(2024, 1, 16, 14, 30, 0),
            RentalHours = 48
        },
        new()
        {
            Client = Clients[2],
            Car = Cars[2],
            RentalStart = new DateTime(2024, 1, 17, 9, 15, 0),
            RentalHours = 12
        },
        new()
        {
            Client = Clients[3],
            Car = Cars[3],
            RentalStart = new DateTime(2024, 1, 18, 16, 45, 0),
            RentalHours = 72
        },
        new()
        {
            Client = Clients[4],
            Car = Cars[4],
            RentalStart = new DateTime(2024, 1, 19, 11, 0, 0),
            RentalHours = 36
        },
        new()
        {
            Client = Clients[5],
            Car = Cars[5],
            RentalStart = new DateTime(2024, 1, 20, 13, 20, 0),
            RentalHours = 24
        },
        new()
        {
            Client = Clients[6],
            Car = Cars[6],
            RentalStart = new DateTime(2024, 1, 21, 8, 0, 0),
            RentalHours = 60
        },
        new()
        {
            Client = Clients[7],
            Car = Cars[7],
            RentalStart = new DateTime(2024, 1, 15, 15, 30, 0),
            RentalHours = 18
        },
        new()
        {
            Client = Clients[8],
            Car = Cars[8],
            RentalStart = new DateTime(2024, 1, 16, 12, 0, 0),
            RentalHours = 42
        },
        new()
        {
            Client = Clients[9],
            Car = Cars[9],
            RentalStart = new DateTime(2024, 1, 17, 17, 45, 0),
            RentalHours = 30
        },
        new() //
        {
            Client = Clients[10],
            Car = Cars[10],
            RentalStart = new DateTime(2024, 1, 18, 10, 15, 0),
            RentalHours = 24
        },
        new()
        {
            Client = Clients[11],
            Car = Cars[11],
            RentalStart = new DateTime(2024, 1, 19, 14, 0, 0),
            RentalHours = 54
        }
    ];
}


