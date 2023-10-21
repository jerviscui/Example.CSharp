using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AuthorizationTest;

public class MinimumAgeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "MinimumAgeScheme";

    private static int _count;

    /// <inheritdoc />
    public MinimumAgeAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    /// <inheritdoc />
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        _count++;

        switch (_count % 3)
        {
            case 1:
                //response status is 401
                return Task.FromResult(AuthenticateResult.NoResult());
            case 2:
                //response status is 401
                return Task.FromResult(AuthenticateResult.Fail("MinimumAge wrong"));
            case 0:
                {
                    //Issuer default "LOCAL AUTHORITY"
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                        new[] { new Claim(ClaimTypes.DateOfBirth, "1990-1-1") }));
                    var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            default:
                return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
