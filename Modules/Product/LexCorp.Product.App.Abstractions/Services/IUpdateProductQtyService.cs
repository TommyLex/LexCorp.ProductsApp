using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using System.Threading.Tasks;

namespace LexCorp.Product.App.Abstractions.Services
{
  /// <summary>
  /// Defines the contract for a service that handles updating product quantities.
  /// </summary>
  public interface IUpdateProductQtyService
  {
    /// <summary>
    /// Updates the quantity of a product.
    /// </summary>
    /// <param name="productUpdateQtyDto">The product quantity update data transfer object.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the updated <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    Task<ResultInfoDto<ProductDetailDto>> UpdateProductQtyAsync(ProductUpdateQtyDto productUpdateQtyDto);
  }
}