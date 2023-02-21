#pragma warning disable ACL1002
using Audacia.Auth.OpenIddict.DependencyInjection;
using Audacia.Auth.OpenIddict.Seeding;
using Blab.Domain.Entities.Security;
using Blab.Identity.Configuration;
using Blab.Identity.Cors;
using Blab.Identity.Database;
using Blab.Identity.ExceptionHandling;
using Blab.Identity.HealthChecks;
using Blab.Identity.Identity;
using Blab.Identity.Logging;
using Blab.Identity.Security;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

builder.Configuration.AddSystemsManager($"/blab/{environment}");

builder.Services.AddConfigOptions(builder.Configuration)
    .AddSwaggerGen()
    .AddDatabaseContext(builder.Configuration)
    .AddIdentityServices()
    .AddOpenIddictServices(builder.Configuration, builder.Environment)
    .AddLoggingServices(builder.Configuration)
    .AddHealthChecks()
    .ConfigureHealthChecks()
    .AddExceptionHandlingServices()
    .AddControllersWithViews()
    .ConfigureOpenIddict<ApplicationUser, int>();

builder.Services.AddCorsServices(builder.Configuration).AddAuthentication();

builder.Services.AddRazorPages();

builder.Services.AddSecurityServices();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddLocalSeeding();
}

var app = builder.Build();

app.UseExceptionHandling(app.Services.GetRequiredService<ILoggerFactory>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!builder.Environment.IsProduction())
{
    app.UseSwagger();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(CorsPolicies.Default);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapHealthChecks("/health", HealthCheckOptionsProvider.CreateHealthCheckOptions(builder.Configuration))
    .AllowAnonymous();

app.Run();
#pragma warning restore ACL1002
