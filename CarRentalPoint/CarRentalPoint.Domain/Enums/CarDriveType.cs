namespace CarRentalPoint.Domain.Enums;

/// <summary>
/// Type of drive system
/// </summary>
public enum CarDriveType
{
    /// <summary>
    /// Front-wheel drive
    /// Power is delivered to the front wheels only
    /// </summary>
    FrontWheel,

    /// <summary>
    /// All-wheel drive (4WD) 
    /// Power is distributed to all four wheels
    /// </summary>
    AllWheel,

    /// <summary>
    /// Rear-wheel drive 
    /// Power is delivered to the rear wheels only
    /// </summary>
    RearWheel
}