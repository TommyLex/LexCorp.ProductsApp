using System;

namespace LexCorp.Product.Dto.Exceptions
{
  /// <summary>
  /// Exception class for cases when product is not found.
  /// </summary>
  public class ProductNotFoundException : Exception
  {
    /// <summary>
    /// Constructor for ProductNotFoundException.
    /// </summary>
    /// <param name="message">Error message</param>
    public ProductNotFoundException(string message)
      :base(message)
    {
    }
  }
}
