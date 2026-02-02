using Workers.Domain.Common;
using Workers.Domain.Entities.Users;

namespace Workers.Domain.Entities.Companies;

/// <summary>
/// Represents a corporate entity (company) that can manage multiple workers.
/// </summary>
public class Company : BaseEntity
{
    /// <summary>
    /// Official name of the company.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Short description of the company's services.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Business identification number (IČO).
    /// </summary>
    public string? RegistrationNumber { get; set; }

    /// <summary>
    /// Tax identification number (DIČ or IČ DPH).
    /// </summary>
    public string? TaxNumber { get; set; }
    
    /// <summary>
    /// ID of the user who owns/represented the company.
    /// </summary>
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    /// <summary>
    /// Company's official website.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// URL to the company logo.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// List of employees or members associated with this company.
    /// </summary>
    public ICollection<CompanyMember> Members { get; set; } = [];
}
}
