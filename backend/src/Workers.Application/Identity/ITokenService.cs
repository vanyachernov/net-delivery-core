using System.Security.Claims;
using Workers.Domain.Entities.Users;

namespace Workers.Application.Identity;

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
