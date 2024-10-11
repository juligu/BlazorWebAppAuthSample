
using System.Security.Claims;

namespace BlazorApp3.Helpers
{
    public interface IUserTokenStorage
    {
        Task ClearTokenAsync(ClaimsPrincipal claimsPrincipal);
        Task<UserToken> GetTokenAsync(ClaimsPrincipal claimsPrincipal);
        Task StoreTokenAsync(ClaimsPrincipal claimsPrincipal, UserToken userToken);
    }
}