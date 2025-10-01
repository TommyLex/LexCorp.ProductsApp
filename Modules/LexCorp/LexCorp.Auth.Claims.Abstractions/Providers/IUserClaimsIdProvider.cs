using System.Security.Claims;

namespace LexCorp.Auth.Claims.Abstractions.Providers
{
  /// <summary>
  /// Interface for providing the user identifier from a claims identity.
  /// </summary>
  public interface IUserClaimsIdProvider
  {
    /// <summary>
    /// Retrieves the user identifier from the specified claims identity.
    /// </summary>
    /// <param name="identity">The claims identity containing user claims.</param>
    /// <returns>The user identifier as a string.</returns>
    string GetUserClaimsId(ClaimsIdentity identity);
  }
}
