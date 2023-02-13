using System.Text.Json;
using Audacia.ExceptionHandling.AspNetCore;

namespace Blab.Api.ExceptionHandling;

/// <summary>
/// Implementation of <see cref="IResponseSerializer"/> that serializes an object using <see cref="System.Text.Json"/>.
/// The default serializer options are used, including camel case property names.
/// </summary>
public class JsonResponseSerializer : IResponseSerializer
{
    /// <inheritdoc />
    public string Serialize(object response)
    {
        return JsonSerializer.Serialize(response);
    }
}
