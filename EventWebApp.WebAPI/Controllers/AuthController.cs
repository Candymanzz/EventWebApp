using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthValidationService _authValidationService;

    public AuthController(IAuthValidationService authValidationService)
    {
      _authValidationService = authValidationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
      var (isValid, result) = await _authValidationService.ValidateLoginAsync(request, cancellationToken);

      if (!isValid)
      {
        if (result is string errorMessage)
        {
          return BadRequest(errorMessage);
        }
        return Ok(result);
      }

      return Ok(result);
    }

    [HttpPost("register-details")]
    public async Task<IActionResult> RegisterDetails([FromBody] RegisterDetailsRequest request, CancellationToken cancellationToken)
    {
      var (isValid, result) = await _authValidationService.ValidateRegisterDetailsAsync(request, cancellationToken);

      if (!isValid)
      {
        return BadRequest(result);
      }

      return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
      var (isValid, result) = await _authValidationService.ValidateRefreshTokenAsync(request, cancellationToken);

      if (!isValid)
      {
        return Unauthorized(result);
      }

      return Ok(result);
    }
  }
}
