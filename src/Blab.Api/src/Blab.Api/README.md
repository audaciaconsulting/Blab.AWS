# Quick Start Guide

This is a guide to get started with the Audacia template API project. Once the checklist below has been actioned this file can be deleted.

The following items are things that you should look at once your project has been generated.

- Add health checks to `HealthChecks/HealthChecksBuilderExtensions.cs`, for example to test database connections
- Specify custom exception handling in `ExceptionHandling/ApplicationBuilderExtensions.cs` and `ExceptionHandling/ExceptionHandlerOptionsBuilderExtensions.cs`
  - Specifically if/when a custom domain exception class is created, modify `ExceptionHandlerOptionsBuilderExtensions` to use it
- Add/modify logging configuration as appropriate in `Logging/ServiceCollectionExtensions.cs`
- Add appropriate Jwt bearer configuration in `Authentication/ServiceCollectionExtensions.cs`
- Set the url of the user interface application in the `EndpointConfig` section of `appsettings.json`
- Check that the CORS policy defined during application startup is appropriate
- Check that the default security headers added in the middleware pipeline are appropriate
- Check that the default X-Robots-Tag header added in the middleware pipeline is appropriate
- Delete `Controllers/ValuesController.cs`
- If sending emails is required, add the [Audacia.Mail](https://dev.azure.com/audacia/Audacia/_git/Audacia.Mail) package
- Use applicable pipeline templates from [Audacia.Build](https://dev.azure.com/audacia/Audacia/_git/Audacia.Build), such as:
   - [OWASP ZAP pipeline](https://dev.azure.com/audacia/Audacia/_git/Audacia.Build?path=/docs/steps/owasp-zap.md) for scheduled security scans
   - [Dependency check pipeline](https://dev.azure.com/audacia/Audacia/_git/Audacia.Build?path=/pipelines/examples/dependency-checks.yaml) for scheduled dependency vulnerability scans