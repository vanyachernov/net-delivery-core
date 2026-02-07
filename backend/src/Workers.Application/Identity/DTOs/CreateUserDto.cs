using Workers.Domain.Enums;

namespace Workers.Application.Identity.DTOs;

public record CreateUserDto(
    string Email, 
    string Password, 
    UserRole Role, 
    string? FirstName, 
    string? LastName, 
    string? PhoneNumber, 
    string? AvatarUrl
);
