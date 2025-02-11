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
  private readonly ILogger<ServicesApiDbRepo> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="ServicesApiDbRepo"/> class.
  /// </summary>
  /// <param name="context">The database context used for data operations.</param>
  /// <param name="logger"></param>
  public ServicesApiDbRepo(
      ServicesDbContext context,
      ILogger<ServicesApiDbRepo> logger
  ) : base(context, logger)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  //********* Loader Validate ***************************//
  protected override async Task SaveEntityAsyncValidate(DbContext context)
  {
    string entityName = await GetCurrentEntity(context);

    _logger.LogInformation($"===> Services SaveEntityAsyncValidate... {entityName}");

    await Task.Run(() =>
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
     });

    await base.SaveEntityAsyncValidate(context);
  }
  //********* Loader Validate ***************************//

  //********* Before Apply Updates **********************//
  protected override async Task BeforeSaveChangesAsync(DbContext context)
  {
    string entityName = await GetCurrentEntity(context);

    _logger.LogInformation($"===> Services BeforeSaveChangesAsync... {entityName}");

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
  //********* Before Apply Updates **********************//

  //********* After Apply Updates ***********************//
  protected override async Task AfterSaveChangesAsync(DbContext context)
  {
    string entityName = await GetCurrentEntity(context);

    _logger.LogInformation($"===> Services AfterSaveChangesAsync... {entityName}");

    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.State == EntityState.Unchanged)
      {
        // Perform actions after entity is added or updated
      }
    }

    await base.AfterSaveChangesAsync(context);
  }
  //********* After Apply Updates ***********************//

}
