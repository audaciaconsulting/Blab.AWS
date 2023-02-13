using System.Globalization;
using Audacia.Core;

namespace Blab.Api.Security;

/// <summary>
/// Implementation of <see cref="ICurrentUserProvider{TId}"/> that gets the user ID from the current user's claims.
/// Assumes an integer user Id, and uses nullable int to allow for the possibility of an unauthenticated user.
/// </summary>
public class ClaimCurrentUserProvider : ICurrentUserProvider<int?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly string _userIdClaimType;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimCurrentUserProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">An instance of <see cref="IHttpContextAccessor"/>.</param>
    /// <param name="userIdClaimType">The claim type to use for a user's Id.</param>
    public ClaimCurrentUserProvider(IHttpContextAccessor httpContextAccessor, string userIdClaimType)
    {
        _httpContextAccessor = httpContextAccessor;
        _userIdClaimType = userIdClaimType;
    }

    /// <inheritdoc />
    public int? GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var user = httpContext?.User;
        if (user is null)
        {
            return null;
        }

        var userIdClaim = user.FindFirst(_userIdClaimType);
        if (userIdClaim is null)
        {
            return null;
        }

        return int.Parse(userIdClaim.Value, CultureInfo.InvariantCulture);
    }
}
