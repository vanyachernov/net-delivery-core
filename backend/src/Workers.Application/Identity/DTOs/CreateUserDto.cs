namespace Workers.Application.Identity.DTOs;

public record CreateUserDto(
    string Email, 
    string Password, 
    string Role, 
    string? FirstName, 
    string? LastName, 
    string? PhoneNumber, 
    string? AvatarUrl
);
