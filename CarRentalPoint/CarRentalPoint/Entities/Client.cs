namespace CarRentalPoint.Entities;

/// <summary>
/// Represents a customer who can rent vehicles from the car rental service
/// Contains personal identification and contact information
/// </summary>
public class Client
{
    /// <summary>
    /// Unique driver's license number used for identification and verification
    /// </summary>
    public string DriverLicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the client in the format "Last Name First Name Middle Name"
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth of the client for age verification and record keeping
    /// </summary>
    public DateTime BirthDate { get; set; }

}