using System.Security.Claims;
using Common;
using Entities.Users;

namespace Service.Infrastructure.Services
{
    public interface IJwtService : IScopedDependency
    {
        Task<AccessToken> GenerateAsync(User user, List<Claim> customeClaims = null, bool driverLogin = false);
    }
}