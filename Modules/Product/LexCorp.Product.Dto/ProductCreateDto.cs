namespace LexCorp.Product.Dto
{
  /// <summary>
  /// Data transfer object for product creation.
  /// </summary>
  public class ProductCreateDto
  {
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL to the main product image.
    /// </summary>
    public string ImageUrl { get; set; }
  }
}
