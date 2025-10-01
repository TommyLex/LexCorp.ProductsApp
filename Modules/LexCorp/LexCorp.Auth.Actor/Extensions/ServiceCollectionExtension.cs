using LexCorp.Auth.Actor.Abstractions.Providers;
using LexCorp.Auth.Actor.Providers;
using LexCorp.Auth.Claims.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;

namespace LexCorp.Auth.Actor.Extensions
{
  /// <summary>
  /// Provides extension methods for registering actor-related services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers services related to actor authentication and claims in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <remarks>
    /// This method registers the following:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Registers actor claims with the type <see cref="Guid"/> using the <see cref="AddAuthActorClaims{TId}"/> method.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Registers the <see cref="IActorGuidProvider"/> interface with its implementation <see cref="ActorGuidProvider"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public static void AddAuthActor<TUser>(this IServiceCollection services)
    where TUser : IdentityUser<Guid>
    {
      services.AddAuthActorClaims<TUser,Guid>(true);
      services.AddTransient<IActorGuidProvider, ActorGuidProvider>();
    }
  }
}