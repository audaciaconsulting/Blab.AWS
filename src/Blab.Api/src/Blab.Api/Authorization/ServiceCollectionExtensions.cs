using Blab.Api.Security;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blab.Api.Authorization;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to authorization.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds authorization with policies to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add authorization policies.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    internal static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
            {
                // Will require authenticated users to all endpoints by default
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            })
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                SetClaimsIdentityOptions(options.ClaimsIdentity);
                SetPasswordOptions(options.Password);
                SetLockoutOptions(options.Lockout);
                SetUserOptions(options.User);
                SetSignInOptions(options.SignIn);
            })
            .AddDefaultTokenProviders()
            .AddDatabaseServices();

        return services;
    }

    private static void AddDatabaseServices(this IdentityBuilder builder)
    {
        builder
            .AddEntityFrameworkStores<BlabDbContext>()
            .AddRoleStore<RoleStore<ApplicationRole, BlabDbContext, int, ApplicationUserRole,
                IdentityRoleClaim<int>>>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, BlabDbContext, int,
                IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityUserToken<int>,
                IdentityRoleClaim<int>>>();
    }

    private static void SetClaimsIdentityOptions(ClaimsIdentityOptions claimsIdentityOptions)
    {
        claimsIdentityOptions.UserIdClaimType = ClaimTypes.UserId;
    }

    private static void SetPasswordOptions(PasswordOptions passwordOptions)
    {
        // n.b. We may want to update password complexity to use zxcvbn for password strength
        //  ref; https://github.com/dropbox/zxcvbn
        //  ref; https://andrewlock.net/creating-custom-password-validators-for-asp-net-core-identity-2/

        // Password settings
        passwordOptions.RequireDigit = true;
        passwordOptions.RequiredLength = 8;
        passwordOptions.RequireNonAlphanumeric = false;
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
