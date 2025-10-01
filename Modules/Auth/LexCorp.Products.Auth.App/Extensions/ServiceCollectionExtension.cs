using LexCorp.Auth.Actor.Extensions;
using LexCorp.Auth.Jwt.Extensions;
using LexCorp.Products.Auth.App.Abstractions.Services;
using LexCorp.Products.Auth.App.Services;
using LexCorp.Products.Data;
using LexCorp.Products.Data.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace LexCorp.Products.Auth.App.Extensions
{
  /// <summary>
  /// Provides extension methods for registering authentication-related services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    private static readonly SymmetricSecurityKey _SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("D9846A89B24E63A5DG9A45C88624C8DA"));

    /// <summary>
    /// Registers authentication services for the LexCorp Products application.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <param name="configuration">The application configuration containing settings for authentication.</param>
    /// <remarks>
    /// This method performs the following registrations:
    /// <list type="bullet">
    /// <item>
    /// <description>Registers actor-related services using <see cref="LexCorp.Auth.Actor.Extensions.ServiceCollectionExtension.AddAuthActor"/>.</description>
    /// </item>
    /// <item>
    /// <description>Registers JWT authentication services using <see cref="LexCorp.Auth.Jwt.Extensions.ServiceCollectionExtension.AddJwtAuth{TUser, TId, TContext}"/>.</description>
    /// </item>
    /// <item>
    /// <description>Registers the <see cref="ILoginService"/>, <see cref="ILogoutService"/>, and <see cref="IRegisterService"/> implementations.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static void AddProductsAuth(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddAuthActor<User>();
      services.AddJwtAuth<User, Guid, AppDbContext>(configuration, _SigningKey, "LexCorp.Products");

      services.AddTransient<ILoginService, LoginService>();
      services.AddTransient<ILogoutService, LogoutService>();
      services.AddTransient<IRegisterService, RegisterService>();
    }
  }
}