using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterDetailsUseCase _registerDetailsUseCase;
    private readonly RefreshTokenUseCase _refreshTokenUseCase;

    public AuthController(
        LoginUseCase loginUseCase,
        RegisterDetailsUseCase registerDetailsUseCase,
        RefreshTokenUseCase refreshTokenUseCase)
    {
      _loginUseCase = loginUseCase;
      _registerDetailsUseCase = registerDetailsUseCase;
      _refreshTokenUseCase = refreshTokenUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
      var result = await _loginUseCase.ExecuteAsync(request, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        400 => BadRequest(data),
        _ => StatusCode(statusCode, data)
      };
    }

    [HttpPost("register-details")]
    public async Task<IActionResult> RegisterDetails([FromBody] RegisterDetailsRequest request, CancellationToken cancellationToken)
    {
      var result = await _registerDetailsUseCase.ExecuteAsync(request, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        400 => BadRequest(data),
        _ => StatusCode(statusCode, data)
      };
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
      var result = await _refreshTokenUseCase.ExecuteAsync(request, cancellationToken);
      var (statusCode, data) = result.ToHttpResponse();

      return statusCode switch
      {
        200 => Ok(data),
        401 => Unauthorized(data),
        _ => StatusCode(statusCode, data)
      };
    }
  }
}
