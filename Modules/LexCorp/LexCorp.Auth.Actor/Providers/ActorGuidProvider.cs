using LexCorp.Auth.Actor.Abstractions.Providers;
using LexCorp.Auth.Claims.Abstractions.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace LexCorp.Auth.Actor.Providers
{
  /// <summary>
  /// Provides the unique identifier of the current actor (e.g., user or system) based on claims.
  /// </summary>
  public class ActorGuidProvider : IActorGuidProvider
  {
    private readonly HttpContext _HttpContext;
    private readonly IUserClaimsIdProvider _UserClaimsIdProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorGuidProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context.</param>
    /// <param name="userClaimsIdProvider">The provider for retrieving the user identifier from claims.</param>
    public ActorGuidProvider(IHttpContextAccessor httpContextAccessor, IUserClaimsIdProvider userClaimsIdProvider)
    {
      _HttpContext = httpContextAccessor.HttpContext;
      _UserClaimsIdProvider = userClaimsIdProvider;
    }

    /// <summary>
    /// Retrieves the unique identifier of the current actor.
    /// </summary>
    /// <returns>
    /// A <see cref="Guid"/> representing the actor's unique identifier, or <c>null</c> if the actor is not available
    /// or the identifier cannot be parsed.
    /// </returns>
    public Guid? GetActorId()
    {
      var claimsIdentity = _HttpContext.User.Identity as ClaimsIdentity;
      if (claimsIdentity == null)
      {
        return null;
      }

      var strId = _UserClaimsIdProvider.GetUserClaimsId(claimsIdentity);
      if (Guid.TryParse(strId, out var userId))
      {
        return userId;
      }

      return null;

    }
  }
}