using Microsoft.AspNetCore.Authentication;

namespace AuthorizationTest;

public class Class1 : IAuthenticationHandler
{
    /// <inheritdoc />
    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public Task<AuthenticateResult> AuthenticateAsync() => throw new NotImplementedException();

    /// <inheritdoc />
    public Task ChallengeAsync(AuthenticationProperties? properties) => throw new NotImplementedException();

    /// <inheritdoc />
    public Task ForbidAsync(AuthenticationProperties? properties) => throw new NotImplementedException();
}
