namespace LexCorp.Auth.Jwt.Options
{
  /// <summary>
  /// Represents configuration options for constructing authentication tokens.
  /// </summary>
  public class ConstructTokenOptions
  {
    /// <summary>
    /// Gets or sets the name of the login provider used for authentication.
    /// </summary>
    /// <remarks>
    /// This value is used to identify the authentication provider when managing tokens.
    /// The default value is "DefaultLoginProvider".
    /// </remarks>
    public string LoginProvider { get; set; } = "DefaultLoginProvider";

    /// <summary>
    /// Gets or sets the name of the token being constructed.
    /// </summary>
    /// <remarks>
    /// This value is used to specify the type of token, such as "JWT".
    /// The default value is "JWT".
    /// </remarks>
    public string TokenName { get; set; } = "JWT";
  }
}