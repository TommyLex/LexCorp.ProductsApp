using LexCorp.LazyLoading.Dto;

namespace LexCorp.LazyLoading.Filter.Validators
{
  /// <summary>
  /// Defines the contract for validating <see cref="LazyLoadingDto"/> instances.
  /// </summary>
  public interface ILazyLoadingDtoValidator
  {
    /// <summary>
    /// Validates the specified <see cref="LazyLoadingDto"/> to ensure all required properties are set.
    /// </summary>
    /// <param name="lazyLoadingDto">The lazy loading DTO to validate.</param>
    /// <returns>
    /// <see langword="true"/> if the DTO is valid; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsValid(LazyLoadingDto lazyLoadingDto);
  }
}