using LexCorp.Auth.Jwt.Abstractions.BusinessLogic;
using LexCorp.Auth.Jwt.Abstractions.Factories;
using LexCorp.Auth.Jwt.Abstractions.Providers;
using LexCorp.Auth.Jwt.BusinessLogic;
using LexCorp.Auth.Jwt.Factories;
using LexCorp.Auth.Jwt.Options;
using LexCorp.Auth.Jwt.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace LexCorp.Auth.Jwt.Extensions
{
  /// <summary>
  /// Extension methods for registering JWT authentication services in the dependency injection container.
  /// </summary>
  /// <remarks>
  /// This method requires the following:
  /// <list type="bullet">
  /// <item>
  /// <description>
  /// Ensure that the `appsettings` configuration contains a section for `JwtOptions` as defined in the <see cref="JwtOptions"/> class.
  /// </description>
  /// </item>
  /// <item>
  /// <description>
  /// Ensure that the `appsettings` configuration contains a section for `ConstructTokenOptions` as defined in the <see cref="ConstructTokenOptions"/> class.
  /// </description>
  /// </item>
  /// <item>
  /// <description>
  /// The `IUserClaimsIdProvider` interface must be registered in the dependency injection container. 
  /// The default implementation is available in the `LexCorp.Auth.Claims` package.
  /// </description>
  /// </item>
  /// </list>
  /// </remarks>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers JWT authentication services, including token generation, validation, and user management.
    /// </summary>
    /// <typeparam name="TUser">The type of the user entity.</typeparam>
    /// <typeparam name="TId">The type of the user identifier.</typeparam>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <param name="configuration">The application configuration containing JWT options.</param>
    /// <param name="signInSecurityKey">The symmetric security key used for signing tokens.</param>
    /// <param name="tokenProvider">The name of the token provider.</param>
    /// <param name="requireConfirmedEmail">Indicates whether email confirmation is required for sign-in.</param>
    /// <remarks>
    /// This method requires the following:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Ensure that the `appsettings` configuration contains a section for `JwtOptions` as defined in the <see cref="JwtOptions"/> class.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Ensure that the `appsettings` configuration contains a section for `ConstructTokenOptions` as defined in the <see cref="ConstructTokenOptions"/> class.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The `IUserClaimsIdProvider` interface must be registered in the dependency injection container. 
    /// The default implementation is available in the `LexCorp.Auth.Claims` package.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public static void AddJwtAuth<TUser, TId, TContext>(this IServiceCollection services, IConfiguration configuration, SymmetricSecurityKey signInSecurityKey, string tokenProvider, bool requireConfirmedEmail = false)
    where TUser : IdentityUser<TId>
    where TId : IEquatable<TId>
    where TContext : DbContext
    {
      services.AddSingleton<IJwtFactory, JwtFactory>();
      services.AddScoped<IGenerateJwtBl, GenerateJwtBl>();
      services.AddTransient<IJwtResultProvider<TUser,TId>, JwtResultProvider<TUser, TId>>();

      services.Configure<ConstructTokenOptions>(opt => configuration.GetSection(nameof(ConstructTokenOptions)).Bind(opt));

      var jwtAppSettingOptions = configuration.GetSection(nameof(JwtOptions));
      services.Configure<JwtOptions>(opt =>
      {
        jwtAppSettingOptions.Bind(opt);
        opt.SigningCredentials = new SigningCredentials(signInSecurityKey, SecurityAlgorithms.HmacSha256);
      });

      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)],

        ValidateAudience = true,
        ValidAudience = jwtAppSettingOptions[nameof(JwtOptions.Audience)],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signInSecurityKey,

        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      var jwtBearerEvents = new JwtBearerEvents
      {
        OnAuthenticationFailed = context =>
        {
          if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
          {
            context.Response.Headers.Add("Token-Expired", "true");
          }
          return Task.CompletedTask;
        }
      };

      services.AddAuthentication(opt =>
      {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(configureOpt =>
      {
        configureOpt.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
        configureOpt.TokenValidationParameters = tokenValidationParameters;
        configureOpt.SaveToken = true;
        configureOpt.Events = jwtBearerEvents;
      });

      services.AddAuthorization();

      services.AddIdentityCore<TUser>(options =>
      {
        options.SignIn.RequireConfirmedEmail = requireConfirmedEmail;
      })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<TContext>()
        .AddTokenProvider(tokenProvider, typeof(DataProtectorTokenProvider<TUser>))
        .AddSignInManager<SignInManager<TUser>>()
        .AddDefaultTokenProviders();
    }
  }
}