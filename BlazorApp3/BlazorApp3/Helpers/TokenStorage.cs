using System.Collections.Concurrent;
using System.Security.Claims;

namespace BlazorApp3.Helpers
{
    public class TokenStorage : IUserTokenStorage
    {
        public ConcurrentDictionary<string, UserToken> _tokens = new();

        public Task<UserToken> GetTokenAsync(ClaimsPrincipal claimsPrincipal)
        {
            var sub = claimsPrincipal.FindFirst("sub")?.Value ?? throw new InvalidOperationException("No Sub Claim");
            if (_tokens.TryGetValue(sub, out var token))
            {
                return Task.FromResult(token);
            }
            return Task.FromResult(new UserToken { Error = "Not Found" });
        }
        public Task StoreTokenAsync(ClaimsPrincipal claimsPrincipal, UserToken userToken)
        {
            var sub = claimsPrincipal.FindFirst("sub")?.Value ?? throw new InvalidOperationException("No Sub Claim");
            _tokens[sub] = userToken;
            return Task.CompletedTask;
        }

        public Task ClearTokenAsync(ClaimsPrincipal claimsPrincipal)
        {
            var sub = claimsPrincipal.FindFirst("sub")?.Value ?? throw new InvalidOperationException("No Sub Claim");
            _tokens.TryRemove(sub, out _);
            return Task.CompletedTask;
        }
    }
}
