using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexCorp.Product.App.Abstractions.Services
{
  /// <summary>
  /// Defines the contract for a service that retrieves a list of products.
  /// </summary>
  public interface IGetProductListService
  {
    /// <summary>
    /// Retrieves a list of all products.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with a collection of <see cref="ProductListDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    Task<ResultInfoDto<IEnumerable<ProductListDto>>> GetProductsListAsync();
  }
}