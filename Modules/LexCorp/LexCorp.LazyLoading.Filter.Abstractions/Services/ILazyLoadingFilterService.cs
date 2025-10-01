using LexCorp.LazyLoading.Dto;
using System.Linq;

namespace LexCorp.LazyLoading.Filter.Abstractions.Services
{
  /// <summary>
  /// Defines the contract for a service that applies lazy loading filters and sorting to queryable collections.
  /// </summary>
  public interface ILazyLoadingFilterService
  {
    /// <summary>
    /// Applies lazy loading filters and sorting to the specified queryable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the queryable collection.</typeparam>
    /// <param name="lazyLoad">The lazy loading metadata containing filters and sorting information.</param>
    /// <param name="query">The queryable collection to which the filters and sorting will be applied.</param>
    /// <returns>
    /// A queryable collection with the applied filters and sorting.
    /// </returns>
    /// <exception cref="System.Exception">
    /// Thrown when the filters or sorting metadata contain invalid or unsupported columns.
    /// </exception>
    IQueryable<T> FilterAndOrder<T>(LazyLoadingDto lazyLoad, IQueryable<T> query)
      where T : class;
  }
}