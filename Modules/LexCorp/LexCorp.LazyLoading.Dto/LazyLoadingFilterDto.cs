namespace LexCorp.LazyLoading.Dto
{
  /// <summary>
  /// Represents metadata for filtering a column in a lazy-loaded table.
  /// </summary>
  public class LazyLoadingFilterDto
  {
    /// <summary>
    /// Gets or sets the value to compare against.
    /// </summary>
    /// <remarks>
    /// The value can be of any type, depending on the property being filtered.
    /// </remarks>
    public object Value { get; set; }

    /// <summary>
    /// Gets or sets the match mode for the filter.
    /// </summary>
    /// <remarks>
    /// The match mode determines how the filter value is compared to the property value. Supported values include:
    /// <list type="bullet">
    /// <item>
    /// <description><c>startsWith</c>: Checks if the property value starts with the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>contains</c>: Checks if the property value contains the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>notContains</c>: Checks if the property value does not contain the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>endsWith</c>: Checks if the property value ends with the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>equals</c> or <c>is</c>: Checks if the property value is equal to the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>notEquals</c>: Checks if the property value is not equal to the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>dateIs</c>: Checks if the property value matches the filter value as a date.</description>
    /// </item>
    /// <item>
    /// <description><c>dateIsNot</c>: Checks if the property value does not match the filter value as a date.</description>
    /// </item>
    /// <item>
    /// <description><c>dateAfter</c>: Checks if the property value is after the filter value as a date.</description>
    /// </item>
    /// <item>
    /// <description><c>dateBefore</c>: Checks if the property value is before the filter value as a date.</description>
    /// </item>
    /// <item>
    /// <description><c>gt</c>: Checks if the property value is greater than the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>lt</c>: Checks if the property value is less than the filter value.</description>
    /// </item>
    /// <item>
    /// <description><c>in</c>: Checks if the property value is in the list of filter values.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public string MatchMode { get; set; }
  }
}