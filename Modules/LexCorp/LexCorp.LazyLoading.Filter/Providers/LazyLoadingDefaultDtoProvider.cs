using LexCorp.LazyLoading.Dto;
using LexCorp.LazyLoading.Dto.Options;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace LexCorp.LazyLoading.Filter.Providers
{
  /// <summary>
  /// Provides default lazy loading options for initializing lazy loading DTOs.
  /// </summary>
  /// <param name="options">The options containing default lazy loading settings.</param>
  /// <remarks>
  /// Initializes a new instance of the <see cref="LazyLoadingDefaultDtoProvider"/> class.
  /// </remarks>

  public class LazyLoadingDefaultDtoProvider(IOptions<DefaultLazyLoadingOptions> options) : ILazyLoadingDefaultDtoProvider
  {
    private readonly DefaultLazyLoadingOptions _DefaultLazyLoadingOptions = options.Value;

    /// <inheritdoc/>
    public LazyLoadingDto GetDefaultLazyLoadingDto()
    {
      return new LazyLoadingDto()
      {
        First = _DefaultLazyLoadingOptions.First,
        Rows = _DefaultLazyLoadingOptions.Rows,
        SortField = _DefaultLazyLoadingOptions.SortField,
        SortOrder = _DefaultLazyLoadingOptions.SortOrder,
        Filters = new Dictionary<string, LazyLoadingFilterDto>(),
        MultiSort = new List<LazyLoadingSortDto>()
      };
    }
  }
}
