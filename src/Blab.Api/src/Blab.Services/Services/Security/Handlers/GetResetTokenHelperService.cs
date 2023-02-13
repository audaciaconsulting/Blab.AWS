using Blab.Domain.Common.Configuration;
using Blab.Services.Services.Security.Interfaces;
using Microsoft.Extensions.Options;

namespace Blab.Services.Services.Security.Handlers;

public class GetResetTokenHelperService : IGetResetTokenHelperService
{
    private readonly HttpClient _client;

    private readonly IOptions<EndpointConfig> _endpointConfig;

    public GetResetTokenHelperService(
        IOptions<EndpointConfig> endpointConfig,
        HttpClient client)
    {
        _endpointConfig = endpointConfig;
        _client = client;
    }

    public async Task<string> ExecuteAsync(int id, string role)
    {
        var url = $"{_endpointConfig.Value.Identity?.OriginalString}/reset/password?id={id}&role={role}";

        using var response = await _client.GetAsync(new Uri(url));
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
