using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using System.Threading.Tasks;

namespace LexCorp.Product.App.Abstractions.Services
{
  /// <summary>
  /// Defines the contract for a service that handles product creation.
  /// </summary>
  public interface ICreateProductService
  {
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productCreateDto">The product creation data transfer object.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the created <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    Task<ResultInfoDto<ProductDetailDto>> CreateProductAsync(ProductCreateDto productCreateDto);
  }
}