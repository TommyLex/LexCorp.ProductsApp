using LexCorp.Auth.Claims.Abstractions.Providers;
using LexCorp.Auth.Claims.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LexCorp.Auth.Claims.Extensions
{
  /// <summary>
  /// Extension methods for registering authentication-related services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers services for handling user claims and actor claims in the dependency injection container.
    /// </summary>
    /// <typeparam name="TId">The type of the user identifier.</typeparam>
    /// <typeparam name="TUser">Type of user model.</typeparam>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <param name="withRoles">If true, roles will be included in claims.</param>
    public static void AddAuthActorClaims<TUser,TId>(this IServiceCollection services, bool withRoles)
      where TUser : IdentityUser<TId>
      where TId : IEquatable<TId>
    {
      services.AddScoped<IUserClaimsIdProvider, UserClaimsActorIdProvider>();

      if (withRoles)
        services.AddScoped<IUserClaimsProvider<TUser,TId>, UserClaimsRoleActorProvider<TUser, TId>>();
      else
        services.AddScoped<IUserClaimsProvider<TUser, TId>, UserClaimsActorProvider<TUser, TId>>();
    }
  }
}
