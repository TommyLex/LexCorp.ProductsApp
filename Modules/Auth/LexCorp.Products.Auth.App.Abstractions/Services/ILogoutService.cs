using LexCorp.Results.Dto;
using System.Threading.Tasks;

namespace LexCorp.Products.Auth.App.Abstractions.Services
{
  /// <summary>
  /// Provides an abstraction for user logout services.
  /// </summary>
  public interface ILogoutService
  {
    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto"/> 
    /// indicating whether the logout was successful and if not it contains error messages.
    /// </returns>
    Task<ResultInfoDto> LogoutAsync();
  }
}