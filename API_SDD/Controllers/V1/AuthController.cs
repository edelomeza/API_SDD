using ApiSDD.Dtos;
using ApiSDD.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

// Controller name follows the versioned resource path; Sonar S101 cannot apply.
#pragma warning disable S101

namespace ApiSDD.Controllers.V1;

/// <summary>nv-05: login slice. Errors flow to the middleware.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("Login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken cancellationToken)
    {
        var response = await _service.LoginAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }
}
