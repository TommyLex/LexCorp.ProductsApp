using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using System;
using System.Threading.Tasks;

namespace LexCorp.Product.App.Abstractions.Services
{
  /// <summary>
  /// Defines the contract for a service that retrieves product details.
  /// </summary>
  public interface IGetProductDetailService
  {
    /// <summary>
    /// Retrieves the details of a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    Task<ResultInfoDto<ProductDetailDto>> GetProductDetailAsync(Guid id);
  }
}