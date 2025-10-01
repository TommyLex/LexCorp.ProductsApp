using System;

namespace LexCorp.Product.Dto
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for the product list.
    /// </summary>
    public class ProductListDto : ProductCreateDto
    {
      /// <summary>
      /// Gets or sets the unique identifier of the product.
      /// </summary>
      public Guid Guid { get; set; }

      /// <summary>
      /// Gets or sets the price of the product.
      /// </summary>
      public decimal? Price { get; set; }

      /// <summary>
      /// Gets or sets the description of the product.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets the quantity of the product in stock.
      /// </summary>
      public int Quantity { get; set; }
    }
  }
