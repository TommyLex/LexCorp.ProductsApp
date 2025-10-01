using System;
using System.Linq.Expressions;

namespace LexCorp.LazyLoading.Filter.Extensions
{
  /// <summary>
  /// Provides generic predicate generation for filtering data based on property values.
  /// </summary>
  public static class GenericPredicateExtension
  {
    /// <summary>
    /// Creates a generic equality predicate for the specified property and value.
    /// </summary>
    /// <typeparam name="T">The type of the object to filter.</typeparam>
    /// <param name="propertyName">The name of the property to compare.</param>
    /// <param name="value">The value to compare the property against.</param>
    /// <returns>
    /// An expression representing the equality predicate.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified property does not exist on the type <typeparamref name="T"/>.
    /// </exception>
    public static Expression<Func<T, bool>> GenericEqual<T>(string propertyName, object value)
    {
      if (string.IsNullOrWhiteSpace(propertyName))
        throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

      var param = Expression.Parameter(typeof(T), "x");
      var property = Expression.PropertyOrField(param, propertyName);

      if (property == null)
        throw new ArgumentException($"Property '{propertyName}' does not exist on type '{typeof(T).Name}'.");

      var constant = Expression.Constant(value);
      var body = Expression.Equal(property, Expression.Convert(constant, property.Type));

      return Expression.Lambda<Func<T, bool>>(body, param);
    }

    /// <summary>
    /// Creates a generic "contains" predicate for the specified string property and value.
    /// </summary>
    /// <typeparam name="T">The type of the object to filter.</typeparam>
    /// <param name="propertyName">The name of the string property to check.</param>
    /// <param name="value">The substring to check for within the property value.</param>
    /// <returns>
    /// An expression representing the "contains" predicate.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified property does not exist on the type <typeparamref name="T"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the specified property is not of type <see cref="string"/>.
    /// </exception>
    public static Expression<Func<T, bool>> GenericContains<T>(string propertyName, string value)
    {
      if (string.IsNullOrWhiteSpace(propertyName))
        throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

      var param = Expression.Parameter(typeof(T), "x");
      var property = Expression.PropertyOrField(param, propertyName);

      if (property == null)
        throw new ArgumentException($"Property '{propertyName}' does not exist on type '{typeof(T).Name}'.");

      if (property.Type != typeof(string))
        throw new InvalidOperationException($"The 'Contains' operation can only be used on string properties. Property '{propertyName}' is of type '{property.Type.Name}'.");

      var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
      var constant = Expression.Constant(value, typeof(string));
      var body = Expression.Call(property, method, constant);

      return Expression.Lambda<Func<T, bool>>(body, param);
    }
  }
}
