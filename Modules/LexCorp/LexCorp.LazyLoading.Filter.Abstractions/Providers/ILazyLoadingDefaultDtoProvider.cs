using LexCorp.LazyLoading.Dto;
using LexCorp.LazyLoading.Dto.Options;

namespace LexCorp.LazyLoading.Filter.Providers
{
  /// <summary>
  /// Defines the contract for a provider that supplies default lazy loading DTOs.
  /// </summary>
  public interface ILazyLoadingDefaultDtoProvider
  {
    /// <summary>
    /// Retrieves the default lazy loading DTO with preconfigured settings.
    /// </summary>
    /// <returns>
    /// Default <see cref="LazyLoadingDto"/> according to <see cref="DefaultLazyLoadingOptions"/>.
    /// </returns>
    LazyLoadingDto GetDefaultLazyLoadingDto();
  }
}