#pragma warning disable ACL1002 // Member or local function contains too many statements
#pragma warning disable CA1506 // Avoid excessive class coupling
using Amazon;
using Amazon.AppConfigData;
using Amazon.Extensions.NETCore.Setup;
using Audacia.Middleware.RobotsMetaTagMiddleware.Extensions;
using Audacia.Middleware.RobotsMetaTagMiddleware.Helpers;
using Audacia.SecureHeadersMiddleware;
using Blab.Api.Authentication;
using Blab.Api.Authorization;
using Blab.Api.Configuration;
using Blab.Api.Cors;
using Blab.Api.Database;
using Blab.Api.ExceptionHandling;
using Blab.Api.Extensions;
using Blab.Api.HealthChecks;
using Blab.Api.IoC.ApplicationServices;
using Blab.Api.Logging;
using Blab.Api.Swagger;
using Blab.DataAccess;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args); 

var environment = builder.Environment.EnvironmentName;

builder.Configuration.AddSystemsManager($"/blab/{environment}");

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

builder.Services.AddControllers();

// Add services to the container.
builder.Services
    .AddConfiguration(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwagger(builder.Configuration)
    .AddTransient<IBlabDbContext, BlabDbContext>()
    .AddDomainServices()
    .AddApplicationServices(builder.Configuration)
    .AddAuthorizationPolicies()
    .AddExceptionHandlingServices()
    .AddCorsServices(builder.Configuration)
    .AddLoggingServices(builder.Configuration)
    .AddAuthenticationServices(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddHealthChecks()
    .ConfigureHealthChecks();

using var app = builder.Build();

app.UseExceptionHandling(app.Services.GetRequiredService<ILoggerFactory>());

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseApiSecurityHeaders();

app.UseXRobotsMetaTagHeader(XRobotsModelBuilder.CreatePrivateAppDefault().Build());

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("SwaggerClient");
        options.OAuthScopes("api");
        options.OAuthUsePkce();
        options.EnablePersistAuthorization();
    });

    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(CorsPolicies.Default);

app.UseHealthCheckUrlRewrite();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        })
        .WithMetadata(new AllowAnonymousAttribute());
});

app.Run();

#pragma warning restore ACL1002 // Member or local function contains too many statements
#pragma warning restore CA1506 // Avoid excessive class coupling
