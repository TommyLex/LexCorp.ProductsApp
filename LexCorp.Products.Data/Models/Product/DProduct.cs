using System;

namespace LexCorp.Products.Data.Models.Product
{
  /// <summary>
  /// Represents a product entity in the system.
  /// </summary>
  public class DProduct
  {
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public Guid Guid { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL to the main product image.
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product in stock.
    /// </summary>
    public int Quantity { get; set; }
  }
}