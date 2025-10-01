namespace LexCorp.Products.Auth.Dto
{
  /// <summary>
  /// Data Transfer Object for user login requests.
  /// </summary>
  public class LoginDto
  {
    /// <summary>
    /// The username of the user attempting to log in.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The password of the user attempting to log in.
    /// </summary>
    public string Password { get; set; }
  }
}