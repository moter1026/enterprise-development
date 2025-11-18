namespace CarRentalPoint.Domain.Entities;

/// <summary>
/// Represents a customer who can rent vehicles from the car rental service
/// Contains personal identification and contact information
/// </summary>
public class Client
{
    /// <summary>
    /// Unique identifier for the client
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique driver's license number used for identification and verification
    /// </summary>
    public required string DriverLicenseNumber { get; set; }

    /// <summary>
    /// Full name of the client in the format "Last Name First Name Middle Name"
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Date of birth of the client for age verification and record keeping
    /// </summary>
    public required DateTime BirthDate { get; set; }

}