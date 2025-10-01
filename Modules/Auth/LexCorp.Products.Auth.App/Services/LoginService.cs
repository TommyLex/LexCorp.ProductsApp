using LexCorp.Auth.Jwt.Abstractions.Providers;
using LexCorp.Jwt.Dto;
using LexCorp.Results.Dto;
using LexCorp.Products.Auth.App.Abstractions.Services;
using LexCorp.Products.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LexCorp.Products.Auth.App.Services
{
  /// <summary>
  /// Service for handling user login.
  /// </summary>
  public class LoginService(UserManager<User> _UserManager, 
    IJwtResultProvider<User, Guid> _JwtResultProvider, 
    ILogger<LoginService> _Logger) : ILoginService
  {
    /// <summary>
    /// Authenticates a user with the specified username and password.
    /// </summary>
    /// <param name="username">The username of the user attempting to log in.</param>
    /// <param name="password">The password of the user attempting to log in.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a JWT token if the login is successful,
    /// or error messages if the login fails.
    /// </returns>
    public async Task<ResultInfoDto<JwtTokenDto>> LoginAsync(string username, string password)
    {
      try
      {
        _Logger.LogInformation($"Attempting to log in user with username: {username}");

        var user = await _UserManager.FindByNameAsync(username);
        if (user == null)
        {
          _Logger.LogWarning($"Login failed for username: {username}. User does not exist.");
          return "Invalid username or password.";
        }

        if (!await _UserManager.CheckPasswordAsync(user, password))
        {
          _Logger.LogWarning($"Login failed for username: {username}. Incorrect password.");
          return "Invalid username or password.";
        }

        // Generate JWT token
        var jwtResult = await _JwtResultProvider.GetJwtToken(user);
        if (!jwtResult.Success)
        {
          _Logger.LogWarning($"Failed to generate JWT token for username: {username}. Errors: {string.Join(", ", jwtResult.Messages)}");
          return new ResultInfoDto<JwtTokenDto>(jwtResult.Messages.ToArray());
        }

        _Logger.LogInformation($"User {username} successfully logged in.");
        return jwtResult;
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, $"An error occurred while logging in user {username}.");
        return "An unexpected error occurred during login.";
      }
    }
  }
}