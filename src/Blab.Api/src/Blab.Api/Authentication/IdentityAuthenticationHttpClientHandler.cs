using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Blab.Api.Authentication;

/// <summary>
/// Handler for authentication on http client.
/// </summary>
public class IdentityAuthenticationHttpClientHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _accessor;

    /// <summary>
    /// Constructor for creating handler for authentication of http client.
    /// </summary>
    /// <param name="accessor"></param>
    public IdentityAuthenticationHttpClientHandler(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    /// <summary>
    /// Check's if the access token has been added as a HTTP header.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _accessor.HttpContext != null
            ? await _accessor.HttpContext.GetTokenAsync("access_token")
            : null;

        // Use the token to make the call.
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}
