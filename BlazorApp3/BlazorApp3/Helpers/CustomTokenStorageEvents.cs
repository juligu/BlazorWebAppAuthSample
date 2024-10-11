using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace BlazorApp3.Helpers;

public class CustomTokenStorageEvents(IUserTokenStorage userTokenStorage) : OpenIdConnectEvents
{
    private readonly IUserTokenStorage _userTokenStorage = userTokenStorage;

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var exp = DateTimeOffset.UtcNow.AddSeconds(double.Parse(
            context.TokenEndpointResponse!.ExpiresIn));

        await _userTokenStorage.StoreTokenAsync(context.Principal!, new UserToken
        {
            AccessToken = context.TokenEndpointResponse.AccessToken,
            AccessTokenType = context.TokenEndpointResponse.TokenType,
            Expiration = exp,
            Scope = context.TokenEndpointResponse.Scope
        });

        await base.TokenValidated(context);
    }
}

