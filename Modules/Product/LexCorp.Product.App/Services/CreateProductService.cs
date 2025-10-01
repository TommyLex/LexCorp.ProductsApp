using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using Microsoft.Extensions.Logging;

namespace LexCorp.Product.App.Services
{
  /// <summary>
  /// Service for creating new products.
  /// </summary>
  /// <param name="_ProductRepository">The product repository for accessing product data.</param>
  /// <param name="_Logger">The logger for logging information and errors.</param>
  public class CreateProductService(IDProductRepository _ProductRepository, ILogger<CreateProductService> _Logger) : ICreateProductService
  {
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productCreateDto">The product creation data transfer object.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the created <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    public async Task<ResultInfoDto<ProductDetailDto>> CreateProductAsync(ProductCreateDto productCreateDto)
    {
      try
      {
        _Logger.LogInformation("Attempting to create a new product.");

        if (!IsValidUrl(productCreateDto.ImageUrl))
        {
          _Logger.LogWarning($"Invalid URL provided for product image: {productCreateDto.ImageUrl}");
          return new ResultInfoDto<ProductDetailDto>("The provided ImageUrl is not a valid URL.");
        }

        var createdProduct = await _ProductRepository.InsertAsync(productCreateDto);

        _Logger.LogInformation($"Successfully created product with ID: {createdProduct.Guid}");
        return new ResultInfoDto<ProductDetailDto>(createdProduct);
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, "An error occurred while creating a new product.");
        return "An unexpected error occurred while creating the product.";
      }
    }

    /// <summary>
    /// Validates whether the given URL is a valid absolute URL.
    /// </summary>
    /// <param name="url">The URL to validate.</param>
    /// <returns><see langword="true"/> if the URL is valid; otherwise, <see langword="false"/>.</returns>
    private bool IsValidUrl(string url)
    {
      return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
             (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
  }
}