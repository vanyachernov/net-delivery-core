namespace Workers.Application.Common.Models;

public record AuthenticationResult(
    bool Succeeded,
    string? Token,
    string? UserId,
    string? UserName,
    string? Role,
    string? Error
)
{
    public static AuthenticationResult Success(string? token, string? userId, string? userName, string? role) =>
        new(true, token, userId, userName, role, null);

    public static AuthenticationResult Failure(string error) =>
        new(false, null, null, null, null, error);
}
