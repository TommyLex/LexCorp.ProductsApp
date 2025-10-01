namespace LexCorp.Products.Auth.Dto
{
  /// <summary>
  /// Data Transfer Object for user registration requests.
  /// </summary>
  public class RegisterDto : LoginDto
  {
    /// <summary>
    /// The email address of the new user.
    /// </summary>
    public string Email { get; set; }
  }
}