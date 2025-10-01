namespace LexCorp.LazyLoading.Dto.Options
{
  /// <summary>
  /// Default options for lazy loading records.
  /// </summary>
  public class DefaultLazyLoadingOptions
  {
    /// <summary>
    /// Gets or sets the index of the first record to load.
    /// </summary>
    /// <remarks>
    /// This property is used for pagination to specify the starting point of the records to load.
    /// </remarks>
    public int First { get; set; }

    /// <summary>
    /// Gets or sets the number of records to load.
    /// </summary>
    /// <remarks>
    /// This property is used for pagination to specify the total number of records to load.
    /// </remarks>
    public int Rows { get; set; }

    /// <summary>
    /// Gets or sets the name of the column to sort by.
    /// </summary>
    /// <remarks>
    /// The column name must match a property of the type being queried.
    /// </remarks>
    public string SortField { get; set; }

    /// <summary>
    /// Gets or sets the sort order for the specified column.
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
    public int SortOrder { get; set; }
  }
}
