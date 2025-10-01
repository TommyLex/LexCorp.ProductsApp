namespace LexCorp.Products.Auth.Dto.Roles
{
  /// <summary>
  /// Represents the different roles available in the system.
  /// </summary>
  public enum RoleType
  {
    /// <summary>
    /// Represents an administrator with full access to all system features and settings.
    /// </summary>
    Admin,

    /// <summary>
    /// Represents a product manager responsible for managing product-related operations.
    /// </summary>
    ProductManager,

    /// <summary>
    /// Represents a standard customer with access to basic features.
    /// </summary>
    Customer,

    /// <summary>
    /// Represents a premium customer with access to additional features and benefits.
    /// </summary>
    GoldCustomer
  }
}