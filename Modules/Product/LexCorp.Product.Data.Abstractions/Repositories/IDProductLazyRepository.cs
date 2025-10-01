using LexCorp.LazyLoading.Dto;
using LexCorp.Product.Dto;
using LexCorp.Product.Dto.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexCorp.Product.Data.Abstractions.Repositories
{
  /// <summary>
  /// Defines the contract for a repository that manages lazy loading.
  /// </summary>
  public interface IDProductLazyRepository
  {
    /// <summary>
    /// Returns a paginated list of products based on the provided lazy loading parameters.
    /// </summary>
    /// <param name="lazyLoad">Parameters for lazy loading.</param>
    /// <returns>Paginated list of products based on the provided lazy loading parameters.</returns>
    Task<LazyLoadingResultDto<ProductListDto[]>> LazyListAsync(LazyLoadingDto lazyLoad);
  }
}