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

  protected override async Task BeforeSaveChangesAsync(DbContext context)
  {
    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.State == EntityState.Added)
      {
        // Perform actions on newly added entities
      }
      else if (entry.State == EntityState.Modified)
      {
        // Perform actions before update
      }
      else if (entry.State == EntityState.Deleted)
      {
        // Perform actions before deletion
      }
    }

    await base.BeforeSaveChangesAsync(context);
  }

  protected override async Task AfterSaveChangesAsync(DbContext context)
  {
    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.State == EntityState.Added)
      {
        // Perform actions after entity is added
      }
      else if (entry.State == EntityState.Modified)
      {
        // Perform actions after entity is modified
      }
      else if (entry.State == EntityState.Deleted)
      {
        // Perform actions after entity is deleted
      }
    }

    await base.AfterSaveChangesAsync(context);
  }
}
