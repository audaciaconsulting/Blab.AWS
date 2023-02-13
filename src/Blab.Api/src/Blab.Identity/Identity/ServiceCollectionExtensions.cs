using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Amazon.SimpleSystemsManagement;
using Audacia.Auth.OpenIddict.Common.Configuration;
using Audacia.Auth.OpenIddict.QuartzCleanup;
using Blab.DataAccess;
using Blab.DataAccess.OpenIddict;
using Blab.Domain.Common.Configuration;
using Blab.Domain.Entities.Security;
using Blab.Identity.Security;
using Microsoft.AspNetCore.Identity;

namespace Blab.Identity.Identity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection serviceCollection)
    {
        var identityBuilder = serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Note the claim types should be constants somewhere, or use the IdentityModel NuGet package
                // with its JwtClaimTypes class
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.UserId;
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Email;
                SetPasswordOptions(options.Password);
                SetLockoutOptions(options.Lockout);
                SetUserOptions(options.User);
                SetSignInOptions(options.SignIn);
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<BlabDbContext>();

        serviceCollection.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = new PathString("/");
            options.AccessDeniedPath = new PathString("/");
            options.LogoutPath = new PathString("/Identity/Account/Logout");
        });

        return serviceCollection;
    }

    public static IServiceCollection AddOpenIddictServices(
        this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        int GetUserIdDelegate(ApplicationUser applicationUser) => applicationUser.Id;

        void OpenIdCoreBuilderDelegate(OpenIddictCoreBuilder openIddictCoreBuilder)
        {
            openIddictCoreBuilder.UseEntityFrameworkCore()
                .UseDbContext<OpenIddictDbContext>()
                .ReplaceDefaultEntities<int>();
        }

        var openIdConnectConfig = configuration.GetSection(nameof(OpenIdConnectConfig)).Get<OpenIdConnectConfig>();

        // var signingCertificate = GetSigningCertificate(configuration);

        var builder = services.AddOpenIddictWithCleanup<ApplicationUser, int>(
                OpenIdCoreBuilderDelegate,
                GetUserIdDelegate,
                openIdConnectConfig,
                environment)
            .AddServer(options =>
            {
                options.DisableAccessTokenEncryption();
                options.SetLogoutEndpointUris("/identity/account/logout");
                // options.AddSigningCertificate(signingCertificate);
                // options.AddEncryptionCertificate(signingCertificate);
            });

        return services;
    }

    // from https://stackoverflow.com/questions/48291665/
    private static X509Certificate2 GetSigningCertificate(IConfiguration configuration)
    {
        // Configuration is the IConfiguration built by the WebHost in my Program.cs and injected into the Startup constructor
        var awsOptions = configuration.GetAWSOptions();
        var ssmClient = awsOptions.CreateServiceClient<IAmazonSimpleSystemsManagement>();

        // This is blocking because this is called during synchronous startup operations of the WebHost--Startup.ConfigureServices()
        var res = ssmClient.GetParametersByPathAsync(new Amazon.SimpleSystemsManagement.Model.GetParametersByPathRequest()
        {
            Path = "/Blab/Id",
            WithDecryption = true
        }).GetAwaiter().GetResult();

        // Decode the certificate
        var base64EncodedCert = res.Parameters.Find(p => p.Name == "/Blab/Id/SigningCert")?.Value;
        var certificatePassword = res.Parameters.Find(p => p.Name == "/Blab/Id/SigningCertSecret")?.Value;

        byte[] decodedPfxBytes = Convert.FromBase64String(base64EncodedCert);

        return new X509Certificate2(decodedPfxBytes, certificatePassword);
    }

    private static void SetPasswordOptions(PasswordOptions passwordOptions)
    {
        // n.b. We may want to update password complexity to use zxcvbn for password strength
        //  ref; https://github.com/dropbox/zxcvbn
        //  ref; https://andrewlock.net/creating-custom-password-validators-for-asp-net-core-identity-2/

        // Password settings
        passwordOptions.RequireDigit = true;
        passwordOptions.RequiredLength = 12;
        passwordOptions.RequireNonAlphanumeric = true;
        passwordOptions.RequireUppercase = true;
        passwordOptions.RequireLowercase = true;

        // the above requirements have been deemed enough for validation, so the unique characters has been left as 1
        passwordOptions.RequiredUniqueChars = 1;
    }

    private static void SetLockoutOptions(LockoutOptions lockoutOptions)
    {
        lockoutOptions.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        lockoutOptions.MaxFailedAccessAttempts = 10;
        lockoutOptions.AllowedForNewUsers = true;
    }

    private static void SetUserOptions(UserOptions userOptions)
    {
        userOptions.RequireUniqueEmail = true;
    }

    private static void SetSignInOptions(SignInOptions signInOptions)
    {
        signInOptions.RequireConfirmedEmail = false;
        signInOptions.RequireConfirmedPhoneNumber = false;
    }
}
