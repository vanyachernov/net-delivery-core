namespace Workers.Domain.Enums;

/// <summary>
/// Defines the primary role of a user in the system.
/// </summary>
public enum UserRole
{
    Guest = 0,
    Client = 1,
    IndividualMaster = 2,
    Company = 3,
    EnterpriseCompany = 4,
    Administrator = 5,
    Moderator = 6
}