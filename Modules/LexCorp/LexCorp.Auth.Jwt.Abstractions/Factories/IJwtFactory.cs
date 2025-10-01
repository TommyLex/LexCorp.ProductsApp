using System.Security.Claims;
using System.Threading.Tasks;

namespace LexCorp.Auth.Jwt.Abstractions.Factories
{
  /// <summary>
  /// Interface for generating JSON Web Tokens (JWT).
  /// </summary>
  public interface IJwtFactory
  {
    /// <summary>
    /// Generates an encoded JWT token based on the provided username and claims identity.
    /// </summary>
    /// <param name="userName">The username for which the token is generated.</param>
    /// <param name="identity">The claims identity containing user claims.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the encoded JWT token as a string.</returns>
    Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
  }
}
