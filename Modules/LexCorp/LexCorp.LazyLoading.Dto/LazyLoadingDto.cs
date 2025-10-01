using System.Collections.Generic;

namespace LexCorp.LazyLoading.Dto
{
  /// <summary>
  /// Specifies the parameters for lazy loading records, including filtering, sorting, and pagination.
  /// </summary>
  public class LazyLoadingDto
  {
    /// <summary>
    /// Gets or sets the index of the first record to load.
    /// </summary>
    /// <remarks>
    /// This property is used for pagination to specify the starting point of the records to load.
    /// </remarks>
    public int? First { get; set; }

    /// <summary>
    /// Gets or sets the index of the last record to load.
    /// </summary>
    /// <remarks>
    /// This property is used for pagination to specify the endpoint of the records to load.
    /// </remarks>
    public int? Last { get; set; }

    /// <summary>
    /// Gets or sets the number of records to load.
    /// </summary>
    /// <remarks>
    /// This property is used for pagination to specify the total number of records to load.
    /// </remarks>
    public int? Rows { get; set; }

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

    /// <summary>
    /// Gets or sets the metadata for sorting by multiple columns.
    /// </summary>
    /// <remarks>
    /// This property allows specifying multiple columns for sorting, each with its own sort order.
    /// </remarks>
    public IEnumerable<LazyLoadingSortDto> MultiSort { get; set; }

    /// <summary>
    /// Gets or sets the metadata for filtering records.
    /// </summary>
    /// <remarks>
    /// The dictionary key represents the column name, and the value contains the filter metadata.
    /// </remarks>
    public Dictionary<string, LazyLoadingFilterDto> Filters { get; set; }

    /// <summary>
    /// Gets or sets the global filter applied to all columns.
    /// </summary>
    /// <remarks>
    /// This property is used to apply a single filter value across all columns in the dataset.
    /// </remarks>
    public string GlobalFilter { get; set; }
  }
}