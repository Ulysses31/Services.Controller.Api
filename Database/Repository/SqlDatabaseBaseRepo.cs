using Microsoft.EntityFrameworkCore;
using Services.Controllers.API.Configuration;
using Services.Controllers.API.Database.Models;

namespace Services.Controllers.API.Database.Repository;

/// <summary>
/// Abstract base repository providing common database operations.
/// </summary>
/// <typeparam name="TEntity">The type of entity being managed.</typeparam>
public abstract class SqlDatabaseBaseRepo<TEntity> where TEntity : class
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SqlDatabaseBaseRepo{TEntity}"/> class.
  /// </summary>
  protected SqlDatabaseBaseRepo()
  {
  }

  #region Sync-Methods

  /// <summary>
  /// Retrieves all entities as a queryable collection.
  /// </summary>
  public abstract IQueryable<TEntity> Filter();

  /// <summary>
  /// Retrieves entities that match the given predicate.
  /// </summary>
  public abstract IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

  /// <summary>
  /// Retrieves all entities as a queryable collection without tracking.
  /// </summary>
  public abstract IQueryable<TEntity> FilterAsNoTracking();

  /// <summary>
  /// Retrieves entities that match the given predicate without tracking.
  /// </summary>
  public abstract IEnumerable<TEntity> FilterAsNoTracking(Func<TEntity, bool> predicate);

  /// <summary>
  /// Retrieves an entity by its identifier.
  /// </summary>
  public abstract TEntity Filter(string id);

  /// <summary>
  /// Creates a new entity in the database.
  /// </summary>
  public abstract TEntity Create(TEntity entity);

  /// <summary>
  /// Deletes an entity from the database.
  /// </summary>
  public abstract TEntity Delete(TEntity entity);

  /// <summary>
  /// Updates an entity that matches the predicate.
  /// </summary>
  public abstract TEntity Update(Func<TEntity, bool> predicate, TEntity entity);

  #endregion Sync-Methods

  #region Async-Methods

  /// <summary>
  /// Asynchronously retrieves entities that match the given pagination query.
  /// </summary>
  /// <param name="paginationQuery">PaginationQuery</param>
  /// <returns></returns>
  // TODO: FilterValidate FilterPaginationAsync 
  public abstract Task<PagedResult<TEntity>> FilterPaginationAsync(PaginationQuery paginationQuery);

  /// <summary>
  /// Asynchronously retrieves all entities as a queryable collection.
  /// </summary>
  // TODO: FilterValidate FilterAsync
  public abstract Task<IQueryable<TEntity>> FilterAsync();

  /// <summary>
  /// Asynchronously retrieves entities that match the given predicate.
  /// </summary>
  // TODO: FilterValidate FilterAsync(Func<TEntity, bool> predicate);
  public abstract Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate);

  /// <summary>
  /// Asynchronously retrieves all entities as a queryable collection without tracking.
  /// </summary>
  // TODO: FilterValidate FilterAsNoTrackingAsync
  public abstract Task<IQueryable<TEntity>> FilterAsNoTrackingAsync();

  /// <summary>
  /// Asynchronously retrieves entities that match the given predicate without tracking.
  /// </summary>
  // TODO: FilterValidate FilterAsNoTrackingAsync(Func<TEntity, bool> predicate);
  public abstract Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate);

  /// <summary>
  /// Asynchronously retrieves an entity by its identifier.
  /// </summary>
  // TODO: FilterValidate FilterAsyncById(string id)
  public abstract Task<TEntity> FilterAsyncById(string id);

  /// <summary>
  /// Asynchronously creates a new entity in the database.
  /// </summary>
  public abstract Task<TEntity> CreateAsync(TEntity entity);

  /// <summary>
  /// Asynchronously updates an entity that matches the predicate.
  /// </summary>
  public abstract Task<TEntity> UpdateAsync(Func<TEntity, bool> predicate, TEntity entity);

  /// <summary>
  /// Asynchronously deletes an entity from the database.
  /// </summary>
  public abstract Task<TEntity> DeleteAsync(TEntity entity);

  #endregion Async-Methods

  #region Save-Methods 

  /// <summary>
  /// Validates the entity before save it to database. 
  /// </summary>
  protected abstract Task SaveEntityValidate(DbContext context);

  /// <summary>
  /// Before Saves changes to the database.
  /// </summary>
  protected abstract Task BeforeSaveChanges(DbContext context);

  /// <summary>
  /// Saves changes to the database.
  /// </summary>
  public abstract int SaveChanges(DbContext context);

  /// <summary>
  /// After Saves changes to the database.
  /// </summary>
  protected abstract Task AfterSaveChanges(DbContext context);

  /// <summary>
  /// Asynchronously validates the entity before save it to database. 
  /// </summary>
  protected abstract Task SaveEntityAsyncValidate(DbContext context);

  /// <summary>
  /// Before Asynchronously Saves changes to the database.
  /// </summary>
  protected abstract Task BeforeSaveChangesAsync(DbContext context);

  /// <summary>
  /// Asynchronously saves changes to the database.
  /// </summary>
  public abstract Task<int> SaveChangesAsync(DbContext context);

  /// <summary>
  /// After Asynchronously Saves changes to the database.
  /// </summary>
  protected abstract Task AfterSaveChangesAsync(DbContext context);

  #endregion Save-Methods
}
