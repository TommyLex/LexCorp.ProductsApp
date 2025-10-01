using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using Microsoft.Extensions.Logging;

namespace LexCorp.Product.App.Services
{
  /// <summary>
  /// Service for retrieving product details.
  /// </summary>
  /// <param name="_ProductRepository">The product repository for accessing product data.</param>
  /// <param name="_Logger">The logger for logging information and errors.</param>
  public class GetProductDetailService(IDProductRepository _ProductRepository, ILogger<GetProductDetailService> _Logger) : IGetProductDetailService
  {
    /// <summary>
    /// Retrieves the details of a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    public async Task<ResultInfoDto<ProductDetailDto>> GetProductDetailAsync(Guid id)
    {
      try
      {
        _Logger.LogInformation($"Attempting to retrieve details for product with ID: {id}");

        var product = await _ProductRepository.GetAsync(id);
        if (product == null)
        {
          _Logger.LogWarning($"Product with ID {id} not found.");
          return "Product not found.";
        }

        _Logger.LogInformation($"Successfully retrieved details for product with ID: {id}");
        return new ResultInfoDto<ProductDetailDto>(product);
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, $"An error occurred while retrieving details for product with ID: {id}");
        return "An unexpected error occurred while retrieving the product details.";
      }
    }
  }
}