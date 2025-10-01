using LexCorp.Auth.Claims.Abstractions.Providers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace LexCorp.Auth.Claims.Providers
{
  /// <summary>
  /// Provides a claims identity for a user, including an "Actor" claim based on the user's ID.
  /// </summary>
  /// <typeparam name="TId">The type of the user identifier.</typeparam>
  public class UserClaimsActorProvider<TUser,TId> : IUserClaimsProvider<TUser, TId>
    where TUser : IdentityUser<TId>
    where TId : IEquatable<TId>
  {
    /// <summary>
    /// Asynchronously retrieves the claims identity for the specified user.
    /// The claims identity includes an "Actor" claim with the user's ID.
    /// </summary>
    /// <param name="user">The user for whom the claims identity is generated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the claims identity for the user.</returns>
    public virtual async Task<ClaimsIdentity> GetClaimsIdentityAsync(TUser user)
    {
      return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"),
        new[] {
          new Claim(ClaimTypes.Actor, user.Id.ToString())
        });
    }
  }
}
