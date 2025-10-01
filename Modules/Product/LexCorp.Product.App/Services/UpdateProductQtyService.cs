using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Product.Dto.Exceptions;
using LexCorp.Results.Dto;
using Microsoft.Extensions.Logging;

namespace LexCorp.Product.App.Services
{
  /// <summary>
  /// Service for updating product quantities.
  /// </summary>
  /// <param name="_ProductRepository">The product repository for accessing product data.</param>
  /// <param name="_Logger">The logger for logging information and errors.</param>
  public class UpdateProductQtyService(IDProductRepository _ProductRepository, ILogger<UpdateProductQtyService> _Logger) : IUpdateProductQtyService
  {
    /// <summary>
    /// Updates the quantity of a product.
    /// </summary>
    /// <param name="productUpdateQtyDto">The product quantity update data transfer object.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the updated <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    public async Task<ResultInfoDto<ProductDetailDto>> UpdateProductQtyAsync(ProductUpdateQtyDto productUpdateQtyDto)
    {
      try
      {
        _Logger.LogInformation($"Attempting to update quantity for product with ID: {productUpdateQtyDto.Guid}");

        var updatedProduct = await _ProductRepository.UpdateProductQty(productUpdateQtyDto);

        _Logger.LogInformation($"Successfully updated quantity for product with ID: {updatedProduct.Guid}");
        return new ResultInfoDto<ProductDetailDto>(updatedProduct);
      }
      catch(ProductNotFoundException ex)
      {
        _Logger.LogError(ex, $"Product not found. ID: {productUpdateQtyDto.Guid}");
        return "Product wasn't found.";
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, $"An error occurred while updating quantity for product with ID: {productUpdateQtyDto.Guid}");
        return "An unexpected error occurred while updating the product quantity.";
      }
    }
  }
}