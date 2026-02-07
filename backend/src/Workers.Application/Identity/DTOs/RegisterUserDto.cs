using Workers.Domain.Enums;

namespace Workers.Application.Identity.DTOs;

public record RegisterUserDto(
    string Email, 
    string Password, 
    string FirstName, 
    string LastName, 
    UserRole Role = UserRole.Client
);
