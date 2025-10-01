using LexCorp.Jwt.Dto;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LexCorp.Auth.Jwt.Abstractions.Providers
{
  /// <summary>
  /// Interface for providing JWT authentication results.
  /// </summary>
  public interface IJwtResultProvider<TUser, TId>
    where TUser : IdentityUser<TId>
    where TId : IEquatable<TId>

  {
    /// <summary>
    /// Retrieves a JWT token based on user.
    /// </summary>
    /// <param name="identityUser">The user for which the token is generated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ResultInfoDto{JwtTokenDto}"/> with the generated JWT token.</returns>
    Task<ResultInfoDto<JwtTokenDto>> GetJwtToken(TUser identityUser);
  }
}
