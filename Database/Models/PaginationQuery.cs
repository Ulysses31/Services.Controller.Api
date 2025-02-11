namespace Services.Controllers.API.Configuration
{
  /// <summary>
  /// Represents the query parameters for pagination, sorting, and searching.
  /// </summary>
  public class PaginationQuery
  {
    /// <summary>
    /// Gets or sets the page number (1-based index). Defaults to 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page. Defaults to 10.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the search term used to filter results.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Gets or sets the column name by which the results should be sorted.
    /// </summary>
    public string? SortColumn { get; set; }

    /// <summary>
    /// Gets or sets the sort direction ("asc" for ascending, "desc" for descending). Defaults to "asc".
    /// </summary>
    public string? SortDirection { get; set; } = "asc";
  }
}
