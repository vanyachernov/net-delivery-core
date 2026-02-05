using Workers.Application.Common.Models;

namespace Workers.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(string IdentityId, string Error)> CreateUserAsync(CreateIdentityUserDto userDto);
}
