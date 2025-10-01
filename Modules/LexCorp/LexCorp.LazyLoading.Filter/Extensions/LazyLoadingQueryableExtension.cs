using LexCorp.LazyLoading.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace LexCorp.LazyLoading.Filter.Extensions
{
  /// <summary>
  /// Provides extension methods for applying lazy loading operations, such as multi-sort, on queryable collections.
  /// </summary>
  public static class LazyLoadingQueryableExtensions
  {
    /// <summary>
    /// Applies multiple sorting operations to the queryable collection based on the provided metadata.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the queryable collection.</typeparam>
    /// <param name="query">The queryable collection to sort.</param>
    /// <param name="multiSortMeta">The metadata defining the fields and sort orders.</param>
    /// <returns>
    /// A queryable collection with the applied sorting operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="query"/> or <paramref name="multiSortMeta"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="multiSortMeta"/> contains invalid or empty field names.
    /// </exception>
    public static IQueryable<T> LazyLoadingMultiSort<T>(
       this IQueryable<T> query,
       IEnumerable<LazyLoadingSortDto> multiSortMeta)
    {
      if (query == null)
        throw new ArgumentNullException(nameof(query), "The queryable collection cannot be null.");

      if (multiSortMeta == null)
        throw new ArgumentNullException(nameof(multiSortMeta), "The sorting metadata cannot be null.");

      bool isFirst = true;

      foreach (var sortMeta in multiSortMeta)
      {
        if (string.IsNullOrWhiteSpace(sortMeta.Field))
          throw new ArgumentException("Field name in sorting metadata cannot be null or empty.", nameof(multiSortMeta));

        var sortExpression = $"{sortMeta.Field} {(sortMeta.Order == 1 ? "" : "desc")}";

        if (isFirst)
        {
          query = query.OrderBy(sortExpression);
          isFirst = false;
        }
        else
        {
          query = ((IOrderedQueryable<T>)query).ThenBy(sortExpression);
        }
      }

      return query;
    }
  }
}