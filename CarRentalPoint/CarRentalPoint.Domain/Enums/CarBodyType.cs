namespace CarRentalPoint.Domain.Enums;

/// <summary>
/// Body type of the vehicle (тип кузова)
/// </summary>
public enum CarBodyType
{
    /// <summary>
    /// Sedan - седан
    /// Three-box configuration with separate engine, passenger, and cargo compartments
    /// </summary>
    Sedan,

    /// <summary>
    /// SUV (Sport Utility Vehicle) - внедорожник
    /// Combines elements of road-going passenger cars with features from off-road vehicles
    /// </summary>
    SUV,

    /// <summary>
    /// Hatchback - хэтчбек
    /// Two-box configuration with a rear door that swings upward to provide access to cargo area
    /// </summary>
    Hatchback,

    /// <summary>
    /// Coupe - купе
    /// Two-door car with a fixed roof and sloping rear line
    /// </summary>
    Coupe,

    /// <summary>
    /// Convertible - кабриолет
    /// Car with a folding or retractable roof
    /// </summary>
    Convertible,

    /// <summary>
    /// Minivan - минивэн
    /// Passenger vehicle designed to maximize interior space for passengers and cargo
    /// </summary>
    Minivan
}