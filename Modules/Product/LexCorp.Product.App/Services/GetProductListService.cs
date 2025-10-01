using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using Microsoft.Extensions.Logging;

namespace LexCorp.Product.App.Services
{
  /// <summary>
  /// Service for retrieving a list of products.
  /// </summary>
  /// <param name="_ProductRepository">The product repository for accessing product data.</param>
  /// <param name="_Logger">The logger for logging information and errors.</param>
  public class GetProductListService(IDProductRepository _ProductRepository, ILogger<GetProductListService> _Logger) : IGetProductListService
  {
    /// <summary>
    /// Retrieves a list of all products.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with a collection of <see cref="ProductListDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    public async Task<ResultInfoDto<IEnumerable<ProductListDto>>> GetProductsListAsync()
    {
      try
      {
        _Logger.LogInformation("Attempting to retrieve the list of products.");

        var products = await _ProductRepository.ListAllAsync();
        _Logger.LogInformation($"Successfully retrieved {products?.Count()} products.");

        return new ResultInfoDto<IEnumerable<ProductListDto>>(products);
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, "An error occurred while retrieving the product list.");
        return "An unexpected error occurred while retrieving the product list.";
      }
    }
  }
}