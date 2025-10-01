using LexCorp.Results.Dto;
using LexCorp.Products.Auth.App.Abstractions.Services;
using LexCorp.Auth.Actor.Abstractions.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using LexCorp.Auth.Jwt.Options;
using LexCorp.Products.Data.Models.Auth;

namespace LexCorp.Products.Auth.App.Services
{
  /// <summary>
  /// Service for handling user logout operations.
  /// </summary>
  public class LogoutService(UserManager<User> _UserManager, 
    IActorGuidProvider _ActorGuidProvider, 
    ILogger<LogoutService> _Logger,
    IOptions<ConstructTokenOptions> constructTokenOptions) : ILogoutService
  {
    private readonly ConstructTokenOptions _ConstructTokenOptions = constructTokenOptions.Value;

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto"/> 
    /// indicating whether the logout was successful and if not it contains error messages.
    /// </returns>
    public async Task<ResultInfoDto> LogoutAsync()
    {
      try
      {
        _Logger.LogInformation("Attempting to log out the current user.");

        var actorId = _ActorGuidProvider.GetActorId();
        if (actorId == null)
        {
          _Logger.LogWarning("Logout failed: No authenticated user found.");
          return "No authenticated user found.";
        }

        var user = await _UserManager.FindByIdAsync(actorId.ToString());
        if (user == null)
        {
          _Logger.LogWarning($"Logout failed: User with ID {actorId} not found.");
          return "User not found.";
        }

        var result = await _UserManager.RemoveAuthenticationTokenAsync(user, _ConstructTokenOptions.LoginProvider, _ConstructTokenOptions.TokenName);
        if (!result.Succeeded)
        {
          _Logger.LogWarning($"Failed to remove authentication token for user {actorId}. Errors: {string.Join(", ", result.Errors)}");
          return "Failed to remove authentication token.";
        }

        _Logger.LogInformation($"User with ID {actorId} successfully logged out.");
        return new ResultInfoDto(true, "User successfully logged out.");
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, "An error occurred during logout.");
        return "An unexpected error occurred during logout.";
      }
    }
  }
}