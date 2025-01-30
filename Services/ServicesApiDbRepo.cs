using Microsoft.EntityFrameworkCore;
using Services.Controllers.API.Database.Contexts;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.Database.Repository;

namespace Services.Controllers.API.Services;

/// <summary>
/// Repository class for interacting with the Services API database.
/// </summary>
public class ServicesApiDbRepo : SqlDatabaseRepo<WeatherForecastDto>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ServicesApiDbRepo"/> class.
  /// </summary>
  /// <param name="context">The database context used for data operations.</param>
  public ServicesApiDbRepo(
      ServicesDbContext context
  ) : base(context)
  {
  }
}
