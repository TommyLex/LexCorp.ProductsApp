using LexCorp.Auth.Jwt.Abstractions.Factories;
using LexCorp.Auth.Jwt.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LexCorp.Auth.Jwt.Factories
{
  /// <summary>
  /// Factory for generating JSON Web Tokens (JWT).
  /// </summary>
  public class JwtFactory : IJwtFactory
  {
    private readonly JwtOptions _JwtOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtFactory"/> class with the specified JWT options.
    /// </summary>
    /// <param name="jwtOptions">The options used to configure the JWT generation.</param>
    /// <exception cref="ArgumentNullException">Thrown if the provided options are null.</exception>
    /// <exception cref="ArgumentException">Thrown if the provided options are invalid.</exception>
    public JwtFactory(IOptions<JwtOptions> jwtOptions)
    {
      _JwtOptions = jwtOptions.Value;
      ThrowIfInvalidOptions(_JwtOptions);
    }

    /// <summary>
    /// Generates an encoded JWT token based on the provided username and claims identity.
    /// </summary>
    /// <param name="userName">The username for which the token is generated.</param>
    /// <param name="identity">The claims identity containing user claims.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the encoded JWT token as a string.</returns>
    public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
    {
      var claims = new List<Claim>()
      {
        new Claim(JwtRegisteredClaimNames.Sub, userName),
        new Claim(JwtRegisteredClaimNames.Jti, await _JwtOptions.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_JwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
      };

      claims.AddRange(identity.Claims);

      var jwt = new JwtSecurityToken(
          issuer: _JwtOptions.Issuer,
          audience: _JwtOptions.Audience,
          claims: claims,
          notBefore: _JwtOptions.NotBefore,
          expires: _JwtOptions.Expiration,
          signingCredentials: _JwtOptions.SigningCredentials);

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }

    /// <summary>
    /// Converts a <see cref="DateTime"/> to the number of seconds since the Unix epoch (January 1, 1970, midnight UTC).
    /// </summary>
    /// <param name="date">The date to convert.</param>
    /// <returns>The number of seconds since the Unix epoch.</returns>
    private static long ToUnixEpochDate(DateTime date) =>
      (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    /// <summary>
    /// Validates the provided JWT options and throws an exception if they are invalid.
    /// </summary>
    /// <param name="options">The JWT options to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown if the options or required properties are null.</exception>
    /// <exception cref="ArgumentException">Thrown if the options contain invalid values.</exception>
    private static void ThrowIfInvalidOptions(JwtOptions options)
    {
      if (options == null) throw new ArgumentNullException(nameof(options));

      if (options.ValidFor <= TimeSpan.Zero)
      {
        throw new ArgumentException("The token validity duration must be a non-zero TimeSpan.", nameof(JwtOptions.ValidFor));
      }

      if (options.SigningCredentials == null)
      {
        throw new ArgumentNullException(nameof(JwtOptions.SigningCredentials), "Signing credentials must be provided.");
      }

      if (options.JtiGenerator == null)
      {
        throw new ArgumentNullException(nameof(JwtOptions.JtiGenerator), "A JTI generator function must be provided.");
      }
    }
  }
}