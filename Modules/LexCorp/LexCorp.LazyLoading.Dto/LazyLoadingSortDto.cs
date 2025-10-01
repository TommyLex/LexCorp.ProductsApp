namespace LexCorp.LazyLoading.Dto
{
  /// <summary>
  /// Represents metadata for sorting a queryable collection in a lazy loading operation.
  /// </summary>
  public class LazyLoadingSortDto
  {
    /// <summary>
    /// Gets or sets the name of the field to sort by.
    /// </summary>
    /// <remarks>
    /// The field name must match a property of the type being queried.
    /// </remarks>
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the sort order for the field.
    /// </summary>
    /// <remarks>
    /// The value should be:
    /// <list type="bullet">
    /// <item>
    /// <description><c>1</c> for ascending order.</description>
    /// </item>
    /// <item>
    /// <description><c>-1</c> for descending order.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public int Order { get; set; }
  }
}