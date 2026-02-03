namespace Workers.Application.Users.DTOs;

using Workers.Domain.Enums;


public record UserDto(
    Guid Id,
    string Email,
    string PhoneNumber,
    string? FirstName,
    string? LastName,
    UserRole Role,
    bool IsEmailVerified,
    bool IsPhoneVerified,
    string? AvatarUrl
);  