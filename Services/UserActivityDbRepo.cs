using Services.Controllers.API.Database.Contexts;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.Database.Repository;

namespace Services.Controllers.API.Services;

/// <summary>
/// Repository class for interacting with the User Activity Log database.
/// </summary>
public class UserActivityDbRepo : SqlDatabaseRepo<UserActivityLogDto>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserActivityDbRepo"/> class.
  /// </summary>
  /// <param name="context">The database context used for data operations.</param>
  public UserActivityDbRepo(
      ServicesDbContext context
  ) : base(context)
  {
  }
}
