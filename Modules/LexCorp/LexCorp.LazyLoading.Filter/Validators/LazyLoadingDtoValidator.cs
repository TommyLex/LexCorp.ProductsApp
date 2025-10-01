using LexCorp.LazyLoading.Dto;
using System.Collections.Generic;

namespace LexCorp.LazyLoading.Filter.Validators
{
  /// <summary>
  /// Provides validation for <see cref="LazyLoadingDto"/> to ensure all required properties are set.
  /// </summary>
  public class LazyLoadingDtoValidator : ILazyLoadingDtoValidator
  {
    /// <summary>
    /// Validates the specified <see cref="LazyLoadingDto"/> to ensure all required properties are set.
    /// </summary>
    /// <param name="lazyLoadingDto">The lazy loading DTO to validate.</param>
    /// <returns>
    /// <see langword="true"/> if the DTO is valid; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValid(LazyLoadingDto lazyLoadingDto)
    {
      if (lazyLoadingDto == null)
      {
        return false;
      }
      if (!lazyLoadingDto.First.HasValue || !lazyLoadingDto.Rows.HasValue)
      {
        return false;
      }

      if (lazyLoadingDto.Filters == null)
      {
        lazyLoadingDto.Filters = new Dictionary<string, LazyLoadingFilterDto>();
      }

      if (lazyLoadingDto.MultiSort == null)
      {
        lazyLoadingDto.MultiSort = new List<LazyLoadingSortDto>();
      }

      return true;
    }
  }
}