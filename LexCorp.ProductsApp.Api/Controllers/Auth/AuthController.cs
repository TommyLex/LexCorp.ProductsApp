using Asp.Versioning;
using LexCorp.Jwt.Dto;
using LexCorp.Results.Dto;
using LexCorp.Products.Auth.App.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LexCorp.Products.Auth.Dto;

namespace LexCorp.ProductsApp.Api.Controllers.Auth
{
  /// <summary>
  /// Controller for handling authentication-related operations.
  /// </summary>
  /// <param name="_LoginService">Service for handling user login.</param>
  /// <param name="_LogoutService">Service for handling user logout.</param>
  /// <param name="_RegisterService">Service for handling user registration.</param>
  [ApiController]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiVersion("1.0")]
  public class AuthController(ILoginService _LoginService, ILogoutService _LogoutService, IRegisterService _RegisterService) : ControllerBase
  {
    /// <summary>
    /// Logs in a user and returns a JWT token.
    /// </summary>
    /// <param name="loginDto">The login request containing username and password.</param>
    /// <returns>A JWT token if the login is successful.</returns>
    [HttpPost("[action]")]
    [MapToApiVersion("1.0")]
    [AllowAnonymous]
    public async Task<ActionResult<ResultInfoDto<JwtTokenDto>>> Login([FromBody] LoginDto loginDto)
    {
      var result = await _LoginService.LoginAsync(loginDto.Username, loginDto.Password);
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto">The registration request containing username, password, and email.</param>
    /// <returns>A result indicating whether the registration was successful.</returns>
    [HttpPost("[action]")]
    [MapToApiVersion("1.0")]
    [AllowAnonymous]
    public async Task<ActionResult<ResultInfoDto>> Register([FromBody] RegisterDto registerDto)
    {
      var result = await _RegisterService.RegisterUserAsync(registerDto.Username, registerDto.Password, registerDto.Email);
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <returns>A result indicating whether the logout was successful.</returns>
    [HttpPost("[action]")]
    [MapToApiVersion("1.0")]
    [Authorize]
    public async Task<ActionResult<ResultInfoDto>> Logout()
    {
      var result = await _LogoutService.LogoutAsync();
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }
  }
}