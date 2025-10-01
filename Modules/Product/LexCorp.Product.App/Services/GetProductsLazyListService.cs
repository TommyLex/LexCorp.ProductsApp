using LexCorp.LazyLoading.Dto;
using LexCorp.LazyLoading.Filter.Providers;
using LexCorp.LazyLoading.Filter.Validators;
using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using Microsoft.Extensions.Logging;

namespace LexCorp.Product.App.Services
{
  /// <summary>
  /// Service for retrieving a lazy-loaded list of products.
  /// </summary>
  /// <param name="_LazyLoadingDtoValidator">The validator for validating lazy loading DTOs.</param>
  /// <param name="_LazyLoadingDefaultDtoProvider">The provider for default lazy loading DTOs.</param>
  /// <param name="_DProductRepository">The repository for accessing product data.</param>
  /// <param name="_Logger">The logger for logging information and errors.</param>
  public class GetProductsLazyListService(ILazyLoadingDtoValidator _LazyLoadingDtoValidator,
    ILazyLoadingDefaultDtoProvider _LazyLoadingDefaultDtoProvider,
    IDProductRepository _DProductRepository,
    ILogger<GetProductsLazyListService> _Logger) : IGetProductsLazyListService
  {
    /// <summary>
    /// Retrieves a lazy-loaded list of products based on the provided lazy loading parameters.
    /// </summary>
    /// <param name="lazyLoadingDto">The lazy loading DTO containing filtering, sorting, and pagination parameters.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="LazyLoadingResultDto{T}"/>
    /// with an array of <see cref="ProductListDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    public async Task<LazyLoadingResultDto<ProductListDto[]>> GetProductsLazyListAsync(LazyLoadingDto lazyLoadingDto)
    {
      try
      {
        if (lazyLoadingDto == null)
        {
          lazyLoadingDto = _LazyLoadingDefaultDtoProvider.GetDefaultLazyLoadingDto();
        }
        else
        {
          if (!_LazyLoadingDtoValidator.IsValid(lazyLoadingDto))
          {
            return _GetFailResult($"Parameters for lazy loading are not valid. Obligatory parameters are: {nameof(LazyLoadingDto.First)}, {nameof(LazyLoadingDto.Rows)}");
          }
        }

        return await _DProductRepository.LazyListAsync(lazyLoadingDto);
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, "An error occurred while retrieving the lazy list of products.");
        return _GetFailResult("An error occurred while retrieving the lazy list of products.");
      }
    }

    /// <summary>
    /// Creates a failure result with the specified error message.
    /// </summary>
    /// <param name="errorMessage">The error message to include in the result.</param>
    /// <returns>A <see cref="LazyLoadingResultDto{T}"/> representing the failure result.</returns>
    private LazyLoadingResultDto<ProductListDto[]> _GetFailResult(string errorMessage)
    {
      return new LazyLoadingResultDto<ProductListDto[]>
      {
        Success = false,
        Total = 0,
        Data = null,
        Messages = new List<string> { errorMessage }
      };
    }
  }
}