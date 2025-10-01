using LexCorp.Jwt.Dto;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LexCorp.Auth.Jwt.Abstractions.BusinessLogic
{
  /// <summary>
  /// Business logic for jwt token creation.
  /// </summary>
  public interface IGenerateJwtBl
  {
    /// <summary>
    /// Creates token for user.
    /// </summary>
    /// <param name="identity">User's identity.</param>
    /// <param name="userName">User name.</param>
    /// <returns></returns>
    Task<JwtTokenDto> GenerateJwt(ClaimsIdentity identity, string userName);
  }
}
