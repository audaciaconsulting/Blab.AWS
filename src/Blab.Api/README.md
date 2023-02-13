# Getting Started

See below for the steps you need to take to get started with a template project.

More information about the code within the template project and recommended approaches to things like architecture and unit testing can be found in the docs folder.

## Contents

1. ReadMe - Getting started
    - [System wide private npm feed access](#System-Wide-Private-npm-Feed-Access)
    - [Configuring urls](#Urls)
    - [Database migrations](#Database-Migration)
    - [Auditing](#Auditing)
    - [JWT claims signing certificate configuration](#JWT-Claims-Signing-Certificate-Configuration)
    - [Configuring SSL](#Configuring-SSL)
    - [Pipelines and Azure resource creation](/docs/Pipelines.md)
1. [Overview](/docs/Overview.md)
    - [Overall solution structure](/docs/Overview.md#Overall-Solution-Structure)
1. [Authentication and authorization](/docs/Authentication-and-Authorization.md)
    - [Authentication](/docs/Authentication-and-Authorization.md#Authentication)
    - [Authorization](/docs/Authentication-and-Authorization.md#Authorization)
    - [User management](/docs/Authentication-and-Authorization.md#User-Management)
    - [Lifetimes](/docs/Authentication-and-Authorization.md#Lifetimes)
1. [Http headers](/docs/HTTP-Headers.md)
    - [SEO considerations](/docs/HTTP-Headers.md#SEO-Considerations)
    - [Secure headers](/docs/HTTP-Headers.md#Secure-Headers)
1. [Application logic](/docs/Application-Logic.md)
    - [Application services](/docs/Application-Logic.md#application-services)
    - [Error handling](/docs/Application-Logic.md#error-handling)
    - [Execution pipeline](/docs/Application-Logic.md#execution-pipeline)
1. [Querying data](/docs/Querying-Data.md)
    - [Consuming dbContext](/docs/Querying-Data.md#consuming-%60dbcontext%60)
    - [Simple queries](/docs/Querying-Data.md#simple-queries)
    - [Complex queries](/docs/Querying-Data.md#complex-queries)
    - [Specification pattern](/docs/Querying-Data.md#specification-pattern)
1. [Unit testing](/docs/Unit-Testing.md)
    - [Testing in the domain layer](/docs/Unit-Testing.md#testing-in-the-domain-layer)
    - [Testing in the services layer](/docs/Unit-Testing.md#testing-in-the-services-layer)
    - [Testing of queries](/docs/Unit-Testing.md#testing-of-queries)
    - [Testing in the API layer](/docs/Unit-Testing.md#testing-in-the-api-layer)
    - [Testing in the UI layer](/docs/Unit-Testing.md#testing-in-the-ui-layer)
1. [Validation](/docs/Validation.md)
    - [Model Validation](/docs/Validation.md#Model-Validation)
    - [Application Validation](/docs/Validation.md#Application-Validation)
    - [Clientside Validation](/docs/Validation.md#Clientside-Validation)
1. [Terraform CLI](/docs/Terraform.md)


## System Wide Private npm Feed Access

If this is your first time working with npm, then you will need an access token for the private npm feed. You will know whether this is true if, when installing npm packages, you get auth based failures.

To generate a token:

-   Open the Project in Azure DevOps/VSTS
-   Click on the Artifacts button
-   Click `Connect to Feed`
-   Click `npm`
-   Click `Windows` (if not already selected)
-   Follow the on-screen instructions

See also: [Configuring a project to access the Audacia NPM registry](https://dev.azure.com/audacia/Audacia/_wiki/wikis/Audacia.wiki/259/Configuring-a-project-to-access-the-Audacia-NPM-registry) on the Audacia Wiki.

## Urls

After creating your project, you will need to check the values assigned to the API, Identity and UI projects for HTTPS ports and ensure that these are the same values in the relevant `appsettings.json` files.

### API

In the `appsettings.json` file, check that the following values are correct:

-   `EndpointConfig:Ui`
-   `EndpointConfig:Api`
-   `Authentication:TokenIssuer`

### Identity

In the `appsettings.json` file, check that the following values are correct:

-   `IdentityServerConfig:Clients:UiClientUrl`
-   `IdentityServerConfig:Clients:SwaggerClientUrl`

### UI

In the `environment.local.ts` file, check that the following values are correct:

-   `config.apiBaseUrl`
-   `config.identityUrl`
-   `config.uiUrl`

## Database Migration

You need to run an initial database migration to add the relevant tables to support authentication and authorization.

Run `Update-Database -StartupProject Audacia.Template.Api` inside Package Manager Console (with `Audacia.Template.EntityFramework` selected as the Default Project), or `dotnet ef database update -p Audacia.Template.EntityFramework -s Audacia.Template.Api` if you prefer using the .NET Core CLI. You can also use the VS Code task `Apply Migrations`, to do this press `F1` to open the command pallete, type `Run task`, press `Enter`, search for `Apply Migrations`, press enter.

## Admin User

In order to be able to log in to the application you will need to create a user. Previous versions of the template project automatically seeded a user on application start, however this is not a recommended approach as it has led to test users being created in live environments.

To create a user an initial user you should do the following:

1. Create a user, along with appropriate role(s), via a SQL script, ensuring that the following properties are set
    - `Email` = user to seed
    - `NormalizedEmail` = user to seed, capitalized
    - `EmailConfirmed` = 1
    - `PasswordHash` = any random string
    - `PhoneNumberConfirmed` = 0
    - `TwoFactorEnabled` = 0
    - `LockoutEnabled` = 1
    - `AccessFailedCount` = 0
    - `FirstName` = user's first name
    - `LastName` = user's last name
    - `UserName` = same as `Email`
    - `NormalizedUserName` = same as `NormalizedEmail`
    - `HasGivenTrackingConsent` = whether or not the user has given consent for their activities to be tracked.
    - `HasGivenTrackingConsentOn` = when the user gave consent for their activities to be tracked.
    - `SecurityStamp` = This is a random string that is required for a password to be set. It is updated every time a user's password is updated or a login is removed.
1. Use the built-in 'forgot password' functionality to set the user's password

### Script To Insert An Admin User

```sql
DECLARE @email VARCHAR(128) = '{email}';
DECLARE @firstName VARCHAR(128) = '{first_name}';
DECLARE @lastName VARCHAR(128) = '{last_name}';

CREATE TABLE #insertedUserIds
(
    UserId INT
);

INSERT INTO Security.AspNetUsers
(
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnabled,
    AccessFailedCount,
    FirstName,
    LastName,
    UserName,
    NormalizedUserName,
    HasGivenTrackingConsent,
    HasGivenTrackingConsentOn,
    SecurityStamp
)
OUTPUT INSERTED.Id INTO #insertedUserIds
VALUES
(
    @email,
    UPPER(@email),
    1,
    NEWID(),
    0,
    0,
    1,
    0,
    @firstName,
    @lastName,
    @email,
    UPPER(@email),
    1,
    GETUTCDATE(),
    NEWID()
);

INSERT INTO Security.AspNetUserRoles
(UserId, RoleId)
SELECT
    UserId,
    (SELECT TOP 1 Id FROM Security.AspNetRoles WHERE Name = 'Administrator')
FROM #insertedUserIds

DROP TABLE #insertedUserIds
```


## Auditing

Auditing is disabled by default. In order to enable it, you need to go to appsettings (for both the Identity app and the API) and change the `Enabled` value within the `AuditingOptions` section to `true`.

This is currently working by storing a JSON representation of the changes in the database.

If you want to change the entities and properties that are being audited have a look at the method `DatabaseTriggerExtensions:ConfigureAuditing` for an example. If you just want to disable auditing for a particular operation there is a method you can use `DatabaseContext:BeginDoNotAudit`. An example of this is below:

```csharp
using(_context.BeginDoNotAudit())
{
    _context.Update(entity);
    _context.SaveChangesAsync();
}
```

### Removing

If you want to complete remove it there are a couple of things that need to be done.

1. Go to the `DatabaseContext`
    - Remove the method `BeginDoNotAudit`
    - Remove the `AuditingOptions` from the constructor
    - Remove the property `_ignoreTriggers`
    - Remove the overwritten version of the `SaveChangesAsync` method
2. In the file `DatabaseTriggerExtensions`, remove the method `ApplyAuditing`.
3. In the method `DatabaseServiceCollectionExtensions.AddDbContext` there is a method call on the database context, to `EnableTriggers`. This needs to be removed.

You can then if you want delete the `AuditingOptions` file and section in appsettings as it is no longer used.


## JWT Claims Signing Certificate Configuration

### Generating a PFX File
1. Navigate to the `/tools` folder in a bash terminal. (Best option would be git bash).
2. Run the following commands
```
   bash CreateTokenSigningCert.sh
   bash ConvertTokenSigningCertToPfx.sh
```
3. Please use LastPass to generate a secure password for the .pfx file when prompted. You should then store this password in your projects shared LastPass folder.

### Import the certificate in Azure
1. Open the Azure Portal
2. Navigate to the resource group for your project and environment.
3. Open the Identity App Service.
4. Go to `TLS/SSL Settings` blade then `Private Key Certificates`, and import the generated .pfx file.
5. Set the `IdentityServerConfig:CertificateThumbprint` app setting for the appropriate environment to the thumbprint of the imported certificate.
6. In the Identity app service configuration, add an app setting for the environment variable `WEBSITE_LOAD_CERTIFICATES` with a value of `*`
7. Repeat the above for each environment, with a different certificate per environment

## Configuring SSL
### Angular SSL Generation

1. Navigate to the tools folder in a bash terminal. (Best option would be git bash).
2. Run the following command

```
   bash CreateSSLCert.sh
```

3. Then copy the two generated files to the ssl folder of the Angular project that you are wanting an SSL certificate for. There should be an `ssl` folder in the `Client` folder for the UI

    - server.key
    - server.crt

4. Once the files are present all that you need to do is install the certificate.

    - Double click it
    - Click `Install certificate`
    - Choose to install for the `Current User`
    - Choose to place certificate in the following store
    - Browse and choose `Trusted Root Certification Authorities`
    - Click `Next` and `Finish` and you are done