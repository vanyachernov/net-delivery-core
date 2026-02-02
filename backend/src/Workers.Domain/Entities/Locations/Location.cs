using Workers.Domain.Common;

namespace Workers.Domain.Entities.Locations;

/// <summary>
/// Represents a geographic location (city/district) in Slovakia.
/// </summary>
public class Location : BaseEntity
{
    /// <summary>
    /// Name of the city or town (e.g., "Bratislava").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Administrative region of Slovakia (Kraj).
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// Administrative district (Okres).
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// Postal code (PSČ).
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Latitude for geographic distance calculation.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude for geographic distance calculation.
    /// </summary>
    public double Longitude { get; set; }
}
