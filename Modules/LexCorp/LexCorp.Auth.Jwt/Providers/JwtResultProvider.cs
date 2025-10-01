using LexCorp.Auth.Claims.Abstractions.Providers;
using LexCorp.Auth.Jwt.Abstractions.BusinessLogic;
using LexCorp.Auth.Jwt.Abstractions.Providers;
using LexCorp.Auth.Jwt.Options;
using LexCorp.Jwt.Dto;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LexCorp.Auth.Jwt.Providers
{
  /// <summary>
  /// Provides JWT tokens for users based on their claims and authentication details.
  /// </summary>
  /// <typeparam name="TUser">The type of the user entity.</typeparam>
  /// <typeparam name="TId">The type of the user identifier.</typeparam>
  public class JwtResultProvider<TUser, TId> : IJwtResultProvider<TUser, TId>
    where TUser : IdentityUser<TId>
    where TId : IEquatable<TId>
  {
    private readonly ILogger<JwtResultProvider<TUser, TId>> _Logger;
    private readonly IGenerateJwtBl _GenerateJwtBl;
    private readonly IUserClaimsProvider<TUser, TId> _UserClaimsProvider;
    private readonly UserManager<TUser> _UserManager;
    private readonly ConstructTokenOptions _ConstructTokenOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtResultProvider{TUser, TId}"/> class.
    /// </summary>
    /// <param name="logger">The logger for logging errors and information.</param>
    /// <param name="generateJwtBl">The business logic for generating JWT tokens.</param>
    /// <param name="userClaimsProvider">The provider for retrieving user claims.</param>
    /// <param name="userManager">The user manager for managing user authentication and tokens.</param>
    /// <param name="constructTokenOptions">Options for JWT construction.</param>
    public JwtResultProvider(ILogger<JwtResultProvider<TUser, TId>> logger, 
      IGenerateJwtBl generateJwtBl, 
      IUserClaimsProvider<TUser, TId> userClaimsProvider, 
      UserManager<TUser> userManager, 
      IOptions<ConstructTokenOptions> constructTokenOptions)
    {
      _Logger = logger;
      _GenerateJwtBl = generateJwtBl;
      _UserClaimsProvider = userClaimsProvider;
      _UserManager = userManager;
      _ConstructTokenOptions = constructTokenOptions.Value;
    }

    /// <summary>
    /// Retrieves a JWT token for the specified user based on their login provider and username.
    /// </summary>
    /// <param name="identityUser">User for which to generate jwt.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ResultInfoDto{JwtTokenDto}"/> with the generated JWT token.</returns>
    public async Task<ResultInfoDto<JwtTokenDto>> GetJwtToken(TUser identityUser)
    {
      try
      {
        var identity = await _UserClaimsProvider.GetClaimsIdentityAsync(identityUser);

        var jwt = await _GenerateJwtBl.GenerateJwt(identity, identityUser.UserName);

        await _UserManager.SetAuthenticationTokenAsync(identityUser, _ConstructTokenOptions.LoginProvider, _ConstructTokenOptions.TokenName, jwt.ToString());

        return jwt;
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, "An unexpected error occurred while generating the JWT token.");
        return "An unexpected error occurred while generating the JWT token.";
      }
    }
  }
}
