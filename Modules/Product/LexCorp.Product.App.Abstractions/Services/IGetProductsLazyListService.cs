using LexCorp.LazyLoading.Dto;
using LexCorp.Product.Dto;
using System.Threading.Tasks;

namespace LexCorp.Product.App.Abstractions.Services
{
  /// <summary>
  /// Defines the contract for a service that retrieves a lazy-loaded list of products.
  /// </summary>
  public interface IGetProductsLazyListService
  {
    /// <summary>
    /// Retrieves a lazy-loaded list of products based on the provided lazy loading parameters.
    /// </summary>
    /// <param name="lazyLoadingDto">The lazy loading DTO containing filtering, sorting, and pagination parameters.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="LazyLoadingResultDto{T}"/>
    /// with an array of <see cref="ProductListDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    Task<LazyLoadingResultDto<ProductListDto[]>> GetProductsLazyListAsync(LazyLoadingDto lazyLoadingDto);
  }
}