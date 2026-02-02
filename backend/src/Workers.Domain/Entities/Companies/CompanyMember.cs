using Workers.Domain.Common;
using Workers.Domain.Entities.Users;

namespace Workers.Domain.Entities.Companies;

/// <summary>
/// Link between a user and a company they belong to.
/// </summary>
public class CompanyMember : BaseEntity
{
    /// <summary>
    /// ID of the company.
    /// </summary>
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    
    /// <summary>
    /// ID of the user who is a member of the company.
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Role or position within the company (e.g., "Senior Plumber").
    /// </summary>
    public string? RoleInCompany { get; set; }
}
