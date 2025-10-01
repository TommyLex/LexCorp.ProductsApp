using LexCorp.Auth.Claims.Abstractions.Providers;
using System.Linq;
using System.Security.Claims;

namespace LexCorp.Auth.Claims.Providers
{
  /// <summary>
  /// Provides the user identifier from a claims identity based on the "Actor" claim type.
  /// </summary>
  public class UserClaimsActorIdProvider : IUserClaimsIdProvider
  {
    /// <summary>
    /// Retrieves the user identifier from the specified claims identity.
    /// The identifier is extracted from the claim of type <see cref="ClaimTypes.Actor"/>.
    /// </summary>
    /// <param name="identity">The claims identity containing user claims.</param>
    /// <returns>The user identifier as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no claim of type <see cref="ClaimTypes.Actor"/> is found.</exception>
    public string GetUserClaimsId(ClaimsIdentity identity)
    {
      return identity.Claims.Single(c => c.Type == ClaimTypes.Actor).Value;
    }
  }
}
