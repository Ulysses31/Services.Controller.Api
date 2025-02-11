using Microsoft.EntityFrameworkCore;
using Services.Controllers.API.Database.Models;

namespace Services.Controllers.API.Configuration
{
  /// <summary>
  /// Provides extension methods for IQueryable to support pagination and dynamic sorting.
  /// </summary>
  public static class QueryableExtensions
  {
    /// <summary>
    /// Converts an <see cref="IQueryable{T}"/> to a paginated result asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of items in the query.</typeparam>
    /// <param name="query">The queryable collection to paginate.</param>
    /// <param name="paginationQuery">The pagination parameters including page number, size, search term, and sorting options.</param>
    /// <returns>A <see cref="PagedResult{T}"/> containing the paginated items and metadata.</returns>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query, PaginationQuery paginationQuery)
    {
      if (!string.IsNullOrWhiteSpace(paginationQuery.SearchTerm))
      {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        query = query.Where(
            x => x.ToString().Contains(paginationQuery.SearchTerm)
        );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
      }

      if (!string.IsNullOrEmpty(paginationQuery.SortColumn))
      {
        query = paginationQuery?.SortDirection?.ToLower() == "desc"
            ? query.OrderByDescendingDynamic(paginationQuery.SortColumn)
            : query.OrderByDynamic(paginationQuery!.SortColumn);
      }

      int totalCount = await query.CountAsync();

      var items = await query
          .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
          .Take(paginationQuery.PageSize)
          .ToListAsync();

      return new PagedResult<T>(
          items,
          totalCount,
          paginationQuery.PageNumber,
          paginationQuery.PageSize
      );
    }

    /// <summary>
    /// Dynamically orders an <see cref="IQueryable{T}"/> by a specified column in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of items in the query.</typeparam>
    /// <param name="query">The queryable collection to order.</param>
    /// <param name="columnName">The name of the column to sort by.</param>
    /// <returns>The ordered query.</returns>
    public static IQueryable<T> OrderByDynamic<T>(
        this IQueryable<T> query, string columnName)
    {
      return query.OrderBy(x => EF.Property<object>(x!, columnName));
    }

    /// <summary>
    /// Dynamically orders an <see cref="IQueryable{T}"/> by a specified column in descending order.
    /// </summary>
    /// <typeparam name="T">The type of items in the query.</typeparam>
    /// <param name="query">The queryable collection to order.</param>
    /// <param name="columnName">The name of the column to sort by.</param>
    /// <returns>The ordered query.</returns>
    public static IQueryable<T> OrderByDescendingDynamic<T>(
        this IQueryable<T> query, string columnName)
    {
      return query.OrderByDescending(x => EF.Property<object>(x!, columnName));
    }
  }
}
