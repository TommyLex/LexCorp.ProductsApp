using System;

namespace LexCorp.Product.Dto.Abstractions
{
  /// <summary>
  /// Base class for product updates.
  /// </summary>
  public abstract class AProductUpdateDto
  {
    /// <summary>
    /// Product identifier.
    /// </summary>
    public Guid Guid { get; set; }
  }
}
