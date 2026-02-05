namespace Workers.Application.Common.Models;

public record CreateIdentityUserDto(
    Guid UserId,
    string Email,
    string Password,
    string PhoneNumber,
    string RoleName
);
