using Microsoft.EntityFrameworkCore;

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

  #region Sync Methods

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

  #endregion Sync Methods

  #region Async Methods

  /// <summary>
  /// Asynchronously retrieves all entities as a queryable collection.
  /// </summary>
  public abstract Task<IQueryable<TEntity>> FilterAsync();

  /// <summary>
  /// Asynchronously retrieves entities that match the given predicate.
  /// </summary>
  public abstract Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate);

  /// <summary>
  /// Asynchronously retrieves all entities as a queryable collection without tracking.
  /// </summary>
  public abstract Task<IQueryable<TEntity>> FilterAsNoTrackingAsync();

  /// <summary>
  /// Asynchronously retrieves entities that match the given predicate without tracking.
  /// </summary>
  public abstract Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate);

  /// <summary>
  /// Asynchronously retrieves an entity by its identifier.
  /// </summary>
  public abstract Task<TEntity> FilterAsync(string id);

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

  #endregion Async Methods

  /// <summary>
  /// Saves changes to the database.
  /// </summary>
  public abstract int SaveChanges(DbContext context);

  /// <summary>
  /// Asynchronously saves changes to the database.
  /// </summary>
  public abstract Task<int> SaveChangesAsync(DbContext context);
}
