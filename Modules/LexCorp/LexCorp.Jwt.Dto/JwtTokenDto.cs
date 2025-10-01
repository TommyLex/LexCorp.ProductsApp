using Newtonsoft.Json;
using System;

namespace LexCorp.Jwt.Dto
{
  /// <summary>
  /// Data Transfer Object for an authorization token.
  /// </summary>
  public class JwtTokenDto
  {
    /// <summary>
    /// The identifier of the user.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// The security token containing user data.
    /// </summary>
    [JsonProperty("authToken")]
    public string AuthToken { get; set; }

    /// <summary>
    /// The validity duration of the token in seconds.
    /// </summary>
    [JsonProperty("expiresIn")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// The date and time when the token expires.
    /// </summary>
    [JsonProperty("validUntil")]
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// Returns a serialized JSON string representation of the object.
    /// </summary>
    /// <returns>A JSON string representing the object.</returns>
    public override string ToString()
    {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
  }
}