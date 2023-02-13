using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blab.Api.Swagger.Filters;

/// <summary>
/// Implementation of <see cref="ISchemaFilter"/> that customizes how enums are represented in the schema by including the enum name.
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema == null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Type.IsEnum && schema.Enum.Any())
        {
            // If the item is an enum with values, we want the Swagger document to include the enum names as well as the integer values.
            // The primary use for this is in API automation test projects, which use NSwag to generate DTOs based on the Swagger schema.
            // "x-enumNames" is an extension understood by NSwag to allow both enum names and values to be included in the generated DTOs.
            var enumNames = new OpenApiArray();
            enumNames.AddRange(Enum.GetNames(context.Type).Select(name => new OpenApiString(name) as IOpenApiAny));
            schema.Extensions.Add("x-enumNames", enumNames);
        }
    }
}
