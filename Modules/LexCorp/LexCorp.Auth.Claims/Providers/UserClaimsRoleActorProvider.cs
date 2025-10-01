using LexCorp.Auth.Claims.Abstractions.Providers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LexCorp.Auth.Claims.Providers
{
  /// <summary>
  /// Provides a claims identity for a user, including an "Actor" claim and role claims based on the user's roles.
  /// </summary>
  /// <typeparam name="TId">The type of the user identifier.</typeparam>
  public class UserClaimsRoleActorProvider<TUser,TId> : UserClaimsActorProvider<TUser, TId>
    where TUser : IdentityUser<TId>
    where TId : IEquatable<TId>
  {
    private readonly UserManager<TUser> _UserManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserClaimsRoleActorProvider{TId}"/> class.
    /// </summary>
    /// <param name="userManager">The user manager for retrieving user roles.</param>
    public UserClaimsRoleActorProvider(UserManager<TUser> userManager)
    {
      _UserManager = userManager;
    }

    /// <summary>
    /// Asynchronously retrieves the claims identity for the specified user, including "Actor" and role claims.
    /// </summary>
    /// <param name="user">The user for whom the claims identity is generated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the claims identity for the user.</returns>
    public override async Task<ClaimsIdentity> GetClaimsIdentityAsync(TUser user)
    {
      var identity = await base.GetClaimsIdentityAsync(user);

      var roles = await _UserManager.GetRolesAsync(user);

      foreach (var role in roles)
      {
        identity.AddClaim(new Claim(ClaimTypes.Role, role));
      }

      return identity;
    }
  }
}