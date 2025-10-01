using LexCorp.Jwt.Dto;
using LexCorp.Results.Dto;
using System.Threading.Tasks;

namespace LexCorp.Products.Auth.App.Abstractions.Services
{
  /// <summary>
  /// Provides an abstraction for user login services.
  /// </summary>
  public interface ILoginService
  {
    /// <summary>
    /// Authenticates a user with the specified username and password.
    /// </summary>
    /// <param name="username">The username of the user attempting to log in.</param>
    /// <param name="password">The password of the user attempting to log in.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a JWT token if the login is successful, or error messages if the login fails.</returns>
    Task<ResultInfoDto<JwtTokenDto>> LoginAsync(string username, string password);
  }
}