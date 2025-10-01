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
  /// Service for handling user registration.
  /// </summary>
  public class RegisterService(UserManager<User> _UserManager, ILogger<RegisterService> _Logger) : IRegisterService
  {
    /// <summary>
    /// Registers a new user with the specified username, password, and email.
    /// </summary>
    /// <param name="userName">The username of the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="email">The email address of the new user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result indicates whether the registration was successful
    /// and contains error messages if the registration failed.
    /// </returns>
    public async Task<ResultInfoDto> RegisterUserAsync(string userName, string password, string email)
    {
      try
      {
        _Logger.LogInformation($"Attempting to register a new user with username: {userName} and email: {email}");

        var user = new User
        {
          UserName = userName,
          Email = email
        };

        var result = await _UserManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {

          foreach (var error in result.Errors)
          {
            _Logger.LogWarning($"Failed to register user {userName}: {error.Description}");
          }

          return new ResultInfoDto(false, result.Errors.Select(e => e.Description).ToArray());
        }

        _Logger.LogInformation($"User {userName} successfully registered.");
        return new ResultInfoDto(true, "Successfully registered.");

      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, $"An error occurred while registering user {userName}.");
        return new ResultInfoDto(false, "An unexpected error occurred during registration.");
      }
    }
  }
}