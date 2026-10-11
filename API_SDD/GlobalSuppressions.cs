using System.Diagnostics.CodeAnalysis;

// Mínimo privilegio (constitución §2.2): la superficie es internal + InternalsVisibleTo.
// Solo lo exigido por el framework (controllers/OpenAPI, Program/WAF, mapped
// exceptions per S3871) queda público, cada caso documentado.
// Scope "namespace" no aplica en este toolchain: supresiones por tipo.
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Common.HttpErrorException",
    Justification = "Public exceptions are catchable cross-assembly by design (S3871); mapped, never remoted.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Common.NotFoundException",
    Justification = "Public mapped exception, constructed by tests and future slices.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Common.ForbiddenException",
    Justification = "Public mapped exception, constructed by tests and future slices.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Common.ConflictException",
    Justification = "Public mapped exception, constructed by tests and future slices.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Common.RequestValidationException",
    Justification = "Public mapped exception, constructed by tests and future slices.")]
[assembly: SuppressMessage(
    "Design",
    "CA1032:Implement standard exception constructors",
    Scope = "type",
    Target = "~T:ApiSDD.Common.HttpErrorException",
    Justification = "Abstract base never constructed directly; concrete types carry the full standard set.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Controllers.V1.PingController",
    Justification = "MVC discovers only public controllers.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Controllers.V1.PingResponse",
    Justification = "DTO in a public ActionResult + OpenAPI schema; wire is legacy via JsonPropertyName.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Controllers.V1.ProbeController",
    Justification = "MVC discovers only public controllers.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:ApiSDD.Data.AppDbContext",
    Justification = "Resolved cross-assembly by DatabaseTest + future dotnet-ef design time.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Scope = "type",
    Target = "~T:Program",
    Justification = "WebApplicationFactory<T> requires an accessible entry-point type.")]
[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Scope = "type",
    Target = "~T:ApiSDD.Health.SqlServerHealthCheck",
    Justification = "A health check must translate any failure into Unhealthy (fail-fast reporting); no exception escapes.")]
[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Scope = "type",
    Target = "~T:ApiSDD.Middleware.ExceptionHandlingMiddleware",
    Justification = "Last-resort boundary: every unhandled exception becomes a uniform ErrorResponse; rethrows when the response already started.")]
[assembly: SuppressMessage(
    "Security",
    "S5332:Using http protocol is insecure",
    Scope = "member",
    Target = "~M:Program.<Main>$(System.String[])",
    Justification = "PORT env listen address for containers/dev; TLS terminates at the proxy, HSTS + prod-only HttpsRedirection enforced in pipeline.")]
