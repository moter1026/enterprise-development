using AutoMapper;
using CarRentalPoint.Application.Dtos;
using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Enums;

namespace CarRentalPoint.Application.Mappers;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs.
/// Configures mappings for Car, CarModel, ModelGeneration, Client, and Rental entities.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class
    /// and defines all entity-to-DTO and DTO-to-entity mappings.
    /// </summary>
    public MappingProfile()
    {
        // CarModel mappings
        CreateMap<CarModel, CarModelGetDto>().ReverseMap();

        CreateMap<CarModelEditDto, CarModel>()
            .ForMember(dest => dest.DriveType, opt => opt.MapFrom(src => Enum.Parse<CarDriveType>(src.DriveType)))
            .ForMember(dest => dest.BodyType, opt => opt.MapFrom(src => Enum.Parse<CarBodyType>(src.BodyType)))
            .ForMember(dest => dest.CarClass, opt => opt.MapFrom(src => Enum.Parse<CarClass>(src.CarClass)))
            .ReverseMap()
            .ForMember(dest => dest.DriveType, opt => opt.MapFrom(src => src.DriveType.ToString()))
            .ForMember(dest => dest.BodyType, opt => opt.MapFrom(src => src.BodyType.ToString()))
            .ForMember(dest => dest.CarClass, opt => opt.MapFrom(src => src.CarClass.ToString()));

        // ModelGeneration mappings
        CreateMap<ModelGeneration, ModelGenerationGetDto>()
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
            .ReverseMap();
        CreateMap<ModelGenerationEditDto, ModelGeneration>()
            .ForMember(dest => dest.Model, opt => opt.Ignore())
            .ReverseMap();

        // Car mappings
        CreateMap<Car, CarGetDto>()
            .ForMember(dest => dest.Generation, opt => opt.MapFrom(src => src.Generation))
            .ReverseMap();
        CreateMap<CarEditDto, Car>()
            .ForMember(dest => dest.Generation, opt => opt.Ignore())
            .ReverseMap();

        // Client mappings
        CreateMap<Client, ClientGetDto>().ReverseMap();
        CreateMap<ClientEditDto, Client>().ReverseMap();

        // Rental mappings
        CreateMap<Rental, RentalGetDto>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
            .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car))
            .ReverseMap();
        CreateMap<RentalEditDto, Rental>()
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.Car, opt => opt.Ignore())
            .ReverseMap();
    }
}
