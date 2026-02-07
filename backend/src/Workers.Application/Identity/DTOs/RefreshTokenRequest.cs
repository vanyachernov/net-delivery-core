namespace Workers.Application.Identity.DTOs;

public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken
);
