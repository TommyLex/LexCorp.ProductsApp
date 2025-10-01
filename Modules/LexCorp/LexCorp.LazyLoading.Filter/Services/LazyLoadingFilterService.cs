using LexCorp.LazyLoading.Dto;
using LexCorp.LazyLoading.Filter.Abstractions.Services;
using LexCorp.LazyLoading.Filter.Extensions;
using LinqKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace LexCorp.LazyLoading.Filter.Services
{
  /// <summary>
  /// Service for applying lazy loading filters and sorting to queryable collections.
  /// </summary>
  public class LazyLoadingFilterService : ILazyLoadingFilterService
  {
    /// <inheritdoc/>
    public IQueryable<T> FilterAndOrder<T>(LazyLoadingDto lazyLoad, IQueryable<T> query) where T : class
    {
      var properties = typeof(T).GetProperties().ToList();
      var numericProperties = properties.Where(x => IsNumericType(x.PropertyType)).ToList();

      // Separate global and local filters
      var globalFilters = lazyLoad.Filters?
        .Where(x => x.Key.Contains("global"))
        .Select(x => new string[] { x.Key.Replace("_global", ""), x.Value.Value.ToString() });

      var localFilters = lazyLoad.Filters?
        .Where(x => !x.Key.Contains("global") && !string.IsNullOrEmpty(x.Value.Value?.ToString()));

      // Validate column names in local filters
      var invalidColumns = localFilters.Select(x => x.Key.ToLower())
        .Except(properties.Select(x => x.Name.ToLower()))
        .Where(x => x != "global");

      if (invalidColumns.Any())
      {
        throw new Exception($"Filters contain invalid columns: {string.Join(", ", invalidColumns)}");
      }

      // Apply local filters
      foreach (var filter in localFilters)
      {
        var value = filter.Value.Value;

        if (numericProperties.Any(x => x.Name.Equals(filter.Key, StringComparison.OrdinalIgnoreCase)))
        {
          var propertyType = properties.First(x => x.Name.Equals(filter.Key, StringComparison.OrdinalIgnoreCase)).PropertyType;

          if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
          {
            propertyType = Nullable.GetUnderlyingType(propertyType);
          }

          value = ConvertToType(filter.Value.Value, propertyType);
        }

        query = ApplyFilter(query, filter.Key, filter.Value.MatchMode, value, properties);
      }

      // Apply global filters
      if (globalFilters?.Any() == true)
      {
        var invalidGlobalColumns = globalFilters.Select(x => x[0].ToLower())
          .Except(properties.Select(x => x.Name.ToLower()));

        if (invalidGlobalColumns.Any())
        {
          throw new Exception($"Global filters contain invalid columns: {string.Join(", ", invalidGlobalColumns)}");
        }

        var predicate = PredicateBuilder.New<T>(false);
        foreach (var filter in globalFilters)
        {
          var numericProperty = numericProperties.FirstOrDefault(x => x.Name.Equals(filter[0], StringComparison.OrdinalIgnoreCase));
          if (numericProperty != null)
          {
            if (TryParseValue(numericProperty, filter[1], out var parsedValue))
            {
              predicate = predicate.Or(GenericPredicateExtension.GenericEqual<T>(filter[0], parsedValue));
            }
          }
          else
          {
            predicate = predicate.Or(GenericPredicateExtension.GenericContains<T>(filter[0], filter[1]));
          }
        }

        query = query.Where(predicate);
      }

      // Apply sorting
      query = ApplySorting(query, lazyLoad, properties);

      return query;
    }

    /// <summary>
    /// Applies a filter to the query based on the specified match mode and value.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the queryable collection.</typeparam>
    /// <param name="query">The queryable collection to filter.</param>
    /// <param name="key">The name of the property to filter on.</param>
    /// <param name="matchMode">The match mode (e.g., "startsWith", "contains").</param>
    /// <param name="value">The value to filter by.</param>
    /// <param name="properties">The list of properties of the type <typeparamref name="T"/>.</param>
    /// <returns>The filtered queryable collection.</returns>
    private IQueryable<T> ApplyFilter<T>(IQueryable<T> query, string key, string matchMode, object value, List<PropertyInfo> properties)
    {
      switch (matchMode)
      {
        case "startsWith":
          return query.Where($"{key}.StartsWith(@0)", value);
        case "contains":
          if (properties.Any(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase) &&
                                  (x.PropertyType == typeof(bool) || x.PropertyType == typeof(bool?))))
          {
            if (bool.TryParse(value?.ToString(), out var boolean))
            {
              return query.Where($"{key} = @0", boolean);
            }
          }
          else
          {
            return query.Where($"{key}.Contains(@0)", value);
          }
          break;
        case "notContains":
          return query.Where($"!{key}.Contains(@0)", value);
        case "endsWith":
          return query.Where($"{key}.EndsWith(@0)", value);
        case "equals":
        case "is":
          return query.Where($"{key} = @0", value);
        case "notEquals":
          return query.Where($"{key} != @0", value);
        case "dateIs":
          if (DateTime.TryParse(value?.ToString(), out var dateIs))
          {
            return query.Where($"{key} = @0", dateIs.Date);
          }
          break;
        case "dateIsNot":
          if (DateTime.TryParse(value?.ToString(), out var dateIsNot))
          {
            return query.Where($"{key} <> @0", dateIsNot.Date);
          }
          break;
        case "dateAfter":
          if (DateTime.TryParse(value?.ToString(), out var dateAfter))
          {
            return query.Where($"{key} > @0", dateAfter.Date);
          }
          break;
        case "dateBefore":
          if (DateTime.TryParse(value?.ToString(), out var dateBefore))
          {
            return query.Where($"{key} < @0", dateBefore.Date);
          }
          break;
        case "gt":
          return query.Where($"{key} > @0", value);
        case "lt":
          return query.Where($"{key} < @0", value);
        case "in":
          if (value is Newtonsoft.Json.Linq.JArray jValues)
          {
            var values = jValues.ToObject<string[]>();
            return ApplyInFilter(query, key, values, properties);
          }
          break;
      }

      return query;
    }

    /// <summary>
    /// Applies an "in" filter to the query.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the queryable collection.</typeparam>
    /// <param name="query">The queryable collection to filter.</param>
    /// <param name="key">The name of the property to filter on.</param>
    /// <param name="values">The values to filter by.</param>
    /// <param name="properties">The list of properties of the type <typeparamref name="T"/>.</param>
    /// <returns>The filtered queryable collection.</returns>
    private IQueryable<T> ApplyInFilter<T>(IQueryable<T> query, string key, string[] values, List<PropertyInfo> properties)
    {
      var property = properties.First(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
      var isNullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
      var elementType = isNullable ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;

      var method = typeof(Enumerable).GetMethods()
        .FirstOrDefault(m => m.Name == "Contains" && m.GetParameters().Length == 2)?
        .MakeGenericMethod(elementType);

      var typedValues = Array.ConvertAll(values, value => ConvertToType(value, elementType));
      var typedArray = Array.CreateInstance(elementType, typedValues.Length);
      typedValues.CopyTo(typedArray, 0);

      var parameterExpr = Expression.Parameter(typeof(T), "x");
      var propertyExpr = Expression.Property(parameterExpr, key);
      var valuesExpr = Expression.Constant(typedArray);

      Expression containsExpr = isNullable
        ? Expression.Call(method, valuesExpr, Expression.Convert(propertyExpr, elementType))
        : Expression.Call(method, valuesExpr, propertyExpr);

      var lambdaExpr = Expression.Lambda<Func<T, bool>>(containsExpr, parameterExpr);
      return query.Where(lambdaExpr);
    }

    /// <summary>
    /// Applies sorting to the query based on the lazy load metadata.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the queryable collection.</typeparam>
    /// <param name="query">The queryable collection to sort.</param>
    /// <param name="lazyLoad">The lazy load metadata containing sorting information.</param>
    /// <param name="properties">The list of properties of the type <typeparamref name="T"/>.</param>
    /// <returns>The sorted queryable collection.</returns>
    private IQueryable<T> ApplySorting<T>(IQueryable<T> query, LazyLoadingDto lazyLoad, List<PropertyInfo> properties)
    {
      if (lazyLoad.MultiSort != null && lazyLoad.MultiSort.Any())
      {
        var invalidColumns = lazyLoad.MultiSort
          .Select(x => x.Field)
          .Where(field => !properties.Select(p => p.Name.ToLower()).Contains(field.ToLower()));

        if (invalidColumns.Any())
        {
          throw new Exception($"Invalid columns for sorting: {string.Join(", ", invalidColumns)}");
        }

        return query.LazyLoadingMultiSort(lazyLoad.MultiSort);
      }

      if (!string.IsNullOrEmpty(lazyLoad.SortField))
      {
        if (!properties.Select(x => x.Name.ToLower()).Contains(lazyLoad.SortField.ToLower()))
        {
          throw new Exception($"{lazyLoad.SortField} is not a valid column for sorting.");
        }

        return query.OrderBy($"{lazyLoad.SortField} {(lazyLoad.SortOrder == 1 ? "" : "desc")}");
      }

      return query;
    }

    /// <summary>
    /// Converts a value to the specified type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="newType">The target type.</param>
    /// <returns>The converted value.</returns>
    private object ConvertToType(object value, Type newType)
    {
      if (newType == typeof(Guid))
      {
        return Guid.Parse(value as string);
      }

      return Convert.ChangeType(value, newType);
    }

    /// <summary>
    /// Attempts to parse a value to the specified property type.
    /// </summary>
    /// <param name="propertyInfo">The property information.</param>
    /// <param name="value">The value to parse.</param>
    /// <param name="result">The parsed value.</param>
    /// <returns><see langword="true"/> if the value was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseValue(PropertyInfo propertyInfo, string value, out object result)
    {
      var type = propertyInfo.PropertyType;
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        type = Nullable.GetUnderlyingType(type);
      }

      var converter = TypeDescriptor.GetConverter(type);

      if (converter != null && IsNumericType(type) && value.Contains(","))
      {
        value = value.Replace(',', '.');
      }

      var values = value.Split('.', StringSplitOptions.RemoveEmptyEntries);
      if (converter != null && IsNumericType(type) && values.Length == 1)
      {
        value = values[0];
      }

      if (converter != null && converter.IsValid(value))
      {
        result = converter.ConvertFromString(null, new CultureInfo("en-US"), value);
        return true;
      }

      result = null;
      return false;
    }

    /// <summary>
    /// Determines whether the specified type is a numeric type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><see langword="true"/> if the type is numeric; otherwise, <see langword="false"/>.</returns>
    public static bool IsNumericType(Type type)
    {
      return type == typeof(int) || type == typeof(int?) || type == typeof(decimal) || type == typeof(decimal?) ||
             type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?) ||
             type == typeof(short) || type == typeof(short?) || type == typeof(long) || type == typeof(long?);
    }
  }
}