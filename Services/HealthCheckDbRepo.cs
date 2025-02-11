using Microsoft.EntityFrameworkCore;
using Services.Controllers.API.Database.Contexts;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.Database.Repository;

namespace Services.Controllers.API.Services;

/// <summary>
/// Repository class for interacting with the User Activity Log database.
/// </summary>
public class HealthCheckDbRepo : SqlDatabaseRepo<BaseEntity>
{
  private readonly ServicesDbContext _context;

  /// <summary>
  /// Initializes a new instance of the <see cref="HealthCheckDbRepo"/> class.
  /// </summary>
  /// <param name="context">The database context used for data operations.</param>
  /// <param name="logger">Serilog</param>
  public HealthCheckDbRepo(
      ServicesDbContext context,
      ILogger<HealthCheckDbRepo> logger
  ) : base(context, logger)
  {
    _context = context;
  }

  /// <summary>
  /// Test the SQL script to ensure the database is up and running. 
  /// </summary>
  /// <returns>string</returns>
  public async Task<string> TestSqlScript()
  {
    FormattableString sql = $"SELECT DATETIME('now')";
    var result = await _context.Database.SqlQuery<string>(sql).ToArrayAsync();
    return result.FirstOrDefault()!;
  }
}
