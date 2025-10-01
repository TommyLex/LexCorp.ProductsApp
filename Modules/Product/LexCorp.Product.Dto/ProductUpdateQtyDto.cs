using LexCorp.Product.Dto.Abstractions;

namespace LexCorp.Product.Dto
{
  /// <summary>
  /// Dto for updating quantity of product.
  /// </summary>
  public class ProductUpdateQtyDto : AProductUpdateDto
  {
    /// <summary>
    /// Quantity to update.
    /// </summary>
    public int Quantity { get; set; }
  }
}
