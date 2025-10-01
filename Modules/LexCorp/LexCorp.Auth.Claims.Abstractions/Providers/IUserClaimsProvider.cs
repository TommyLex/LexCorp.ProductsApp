using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LexCorp.Auth.Claims.Abstractions.Providers
{
  /// <summary>
  /// Interface for providing claims identity for a user.
  /// </summary>
  /// <typeparam name="TId">The type of the user identifier.</typeparam>
  public interface IUserClaimsProvider<TUser, TId>
    where TUser : IdentityUser<TId>
    where TId : IEquatable<TId>
  {
    /// <summary>
    /// Asynchronously retrieves the claims identity for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the claims identity is generated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the claims identity for the user.</returns>
    Task<ClaimsIdentity> GetClaimsIdentityAsync(TUser user);
  }
}
