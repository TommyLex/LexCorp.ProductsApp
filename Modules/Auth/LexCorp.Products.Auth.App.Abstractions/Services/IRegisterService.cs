using LexCorp.Results.Dto;
using System.Threading.Tasks;

namespace LexCorp.Products.Auth.App.Abstractions.Services
{
  /// <summary>
  /// Provides an abstraction for user registration services.
  /// </summary>
  public interface IRegisterService
  {
    /// <summary>
    /// Registers a new user with the specified username, password, and email.
    /// </summary>
    /// <param name="username">The username of the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="email">The email address of the new user.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the registration was successful and if not it contains error messages.</returns>
    Task<ResultInfoDto> RegisterUserAsync(string username, string password, string email);
  }
}