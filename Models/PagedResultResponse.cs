using System.Text.Json.Serialization;

namespace Services.Controllers.API.Models
{
  /// <summary>
  /// Represents a paginated result set.
  /// </summary>
  /// <typeparam name="T">The type of items in the result set.</typeparam>
  public class PagedResultResponse<T>
  {
    public PagedResultResponse() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResultResponse{T}"/> class.
    /// </summary>
    /// <param name="items">The list of items for the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public PagedResultResponse(
      List<T> items,
      int totalCount,
      int currentPage,
      int pageSize
    )
    {
      Items = items;
      TotalCount = totalCount;
      CurrentPage = currentPage;
      PageSize = pageSize;
    }

    /// <summary>
    /// Gets or sets the list of items for the current page.
    /// </summary>
    [JsonPropertyName("items")]
    public List<T>? Items { get; set; }

    /// <summary>
    /// Gets or sets the total count of items across all pages.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the current page number (1-based index).
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets the total number of pages based on the total item count and page size.
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
  }
}
