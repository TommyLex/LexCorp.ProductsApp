using LexCorp.Auth.Claims.Abstractions.Providers;
using LexCorp.Auth.Jwt.Abstractions.BusinessLogic;
using LexCorp.Auth.Jwt.Abstractions.Factories;
using LexCorp.Auth.Jwt.Options;
using LexCorp.Jwt.Dto;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LexCorp.Auth.Jwt.BusinessLogic
{
  /// <summary>
  /// Business logic for generating JSON Web Tokens (JWT).
  /// </summary>
  public class GenerateJwtBl : IGenerateJwtBl
  {
    private readonly IJwtFactory JwtFactory;
    private readonly JwtOptions JwtOptions;
    private readonly IUserClaimsIdProvider _UserClaimsIdProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateJwtBl"/> class.
    /// </summary>
    /// <param name="jwtFactory">The factory for generating encoded JWT tokens.</param>
    /// <param name="jwtOptions">The options used to configure JWT generation.</param>
    /// <param name="userClaimsIdProvider">The provider for retrieving the user identifier from claims.</param>
    public GenerateJwtBl(IJwtFactory jwtFactory, IOptions<JwtOptions> jwtOptions, IUserClaimsIdProvider userClaimsIdProvider)
    {
      JwtFactory = jwtFactory;
      JwtOptions = jwtOptions.Value;
      _UserClaimsIdProvider = userClaimsIdProvider;
    }

    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="identity">The claims identity of the user.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated JWT token as a <see cref="JwtTokenDto"/>.</returns>
    public async Task<JwtTokenDto> GenerateJwt(ClaimsIdentity identity, string userName)
    {
      return new JwtTokenDto
      {
        Id = _UserClaimsIdProvider.GetUserClaimsId(identity),
        AuthToken = await JwtFactory.GenerateEncodedToken(userName, identity),
        ExpiresIn = (int)JwtOptions.ValidFor.TotalSeconds,
        ValidUntil = DateTime.Now.AddSeconds((int)JwtOptions.ValidFor.TotalSeconds)
      };
    }
  }
}
