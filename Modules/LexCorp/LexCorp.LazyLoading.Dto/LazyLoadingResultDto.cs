using LexCorp.Results.Dto;

namespace LexCorp.LazyLoading.Dto
{
  /// <summary>
  /// Represents the result of a lazy loading operation, including the total number of items and the operation status.
  /// </summary>
  /// <typeparam name="T">The type of the data included in the result.</typeparam>
  public class LazyLoadingResultDto<T> : ResultInfoDto<T>
  {
    /// <summary>
    /// Gets or sets the total number of items in the result set.
    /// </summary>
    public int Total { get; set; }
  }
}