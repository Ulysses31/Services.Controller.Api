using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Services.Controllers.API.Configuration;
using Services.Controllers.API.Database.Models;

namespace Services.Controllers.API.Database.Repository;

/// <summary>
/// A generic repository class for interacting with a SQL database.
/// </summary>
/// <typeparam name="TEntity">The type of entity being managed.</typeparam>
public class SqlDatabaseRepo<TEntity> : SqlDatabaseBaseRepo<TEntity> where TEntity : class
{
  private const string NullEntity = "You must provide an entity.";

  private readonly DbContext _context;

  /// <summary>
  /// Initializes a new instance of the <see cref="SqlDatabaseRepo{TEntity}"/> class.
  /// </summary>
  /// <param name="context">The database context used for data operations.</param>
  public SqlDatabaseRepo(DbContext context) : base()
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  #region Sync-Methods

  /// <inheritdoc/>
  public override IQueryable<TEntity> Filter()
  {
    return _context.Set<TEntity>();
  }

  /// <inheritdoc/>
  public override IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate)
  {
    return _context.Set<TEntity>().Where(predicate);
  }

  /// <inheritdoc/>
  public override IQueryable<TEntity> FilterAsNoTracking()
  {
    return _context.Set<TEntity>().AsNoTracking();
  }

  /// <inheritdoc/>
  public override IEnumerable<TEntity> FilterAsNoTracking(Func<TEntity, bool> predicate)
  {
    return _context.Set<TEntity>().AsNoTracking().Where(predicate);
  }

  /// <inheritdoc/>
  public override TEntity Filter(string id)
  {
    if (id == null) throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    var entity = _context.Set<TEntity>().Find(Convert.ToInt32(id));
    return entity ?? throw new Exception("Entity not found.");
  }

  /// <inheritdoc/>
  public override TEntity Create(TEntity entity)
  {
    if (entity == null) throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    _context.Set<TEntity>().Add(entity);
    SaveChanges(_context);
    return entity;
  }

  /// <inheritdoc/>
  public override TEntity Delete(TEntity entity)
  {
    if (entity == null) throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    _context.Set<TEntity>().Remove(entity);
    SaveChanges(_context);
    return entity;
  }

  /// <inheritdoc/>
  public override TEntity Update(Func<TEntity, bool> predicate, TEntity entity)
  {
    var entityToUpdate = Filter(predicate).FirstOrDefault() ?? throw new Exception("Entity not found.");
    entityToUpdate = entity ?? throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    _context.Entry(entity).State = EntityState.Modified;
    SaveChanges(_context);
    return entityToUpdate;
  }

  #endregion Sync-Methods


  #region Async-Methods
  /// <inheritdoc/>
  public override async Task<PagedResult<TEntity>> FilterPaginationAsync(
    PaginationQuery paginationQuery
  )
  {
    var query = _context.Set<TEntity>().OrderBy(x => x).AsQueryable();
    return await query.ToPagedResultAsync(paginationQuery);
  }

  /// <inheritdoc/>
  public override async Task<IQueryable<TEntity>> FilterAsync()
  {
    return await Task.FromResult(_context.Set<TEntity>());
  }

  /// <inheritdoc/>
  public override async Task<IEnumerable<TEntity>> FilterAsync(
    Func<TEntity, bool> predicate
  )
  {
    return await Task.FromResult(
      _context.Set<TEntity>().Where(predicate)
    );
  }

  /// <inheritdoc/>
  public override async Task<IQueryable<TEntity>> FilterAsNoTrackingAsync()
  {
    return await Task.FromResult(_context.Set<TEntity>().AsNoTracking());
  }

  /// <inheritdoc/>
  public override async Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate)
  {
    return await Task.FromResult(_context.Set<TEntity>().AsNoTracking().Where(predicate));
  }

  /// <inheritdoc/>
  public override async Task<TEntity> FilterAsync(string id)
  {
    if (id == null) throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    var entity = await _context.Set<TEntity>().FindAsync(Convert.ToInt32(id));
    return entity ?? throw new Exception("Entity not found.");
  }

  /// <inheritdoc/>
  public override async Task<TEntity> CreateAsync(TEntity entity)
  {
    if (entity == null) throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    await _context.Set<TEntity>().AddAsync(entity);
    await SaveChangesAsync(_context);
    return entity;
  }

  /// <inheritdoc/>
  public override async Task<TEntity> UpdateAsync(Func<TEntity, bool> predicate, TEntity entity)
  {
    // var entityToUpdate = (await FilterAsNoTrackingAsync(predicate)).FirstOrDefault() ?? throw new Exception("Entity not found.");
    // entityToUpdate = entity ?? throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    _context.Entry(entity).State = EntityState.Modified;
    await SaveChangesAsync(_context);
    return entity;
  }

  /// <inheritdoc/>
  public override async Task<TEntity> DeleteAsync(TEntity entity)
  {
    if (entity == null) throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    _context.Set<TEntity>().Remove(entity);
    await SaveChangesAsync(_context);
    return entity;
  }

  #endregion Async-Methods

  /// <inheritdoc/>
  public override int SaveChanges(DbContext context)
  {
    int result = 0;

    try
    {
      result = context.SaveChanges();
    }
    catch (DbUpdateConcurrencyException ex)
    {
      _ = HandleConcurrencyException(ex, false);
    }

    return result;
  }

  /// <inheritdoc/>
  public override async Task<int> SaveChangesAsync(DbContext context)
  {
    int result = 0;

    try
    {
      result = await context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException ex)
    {
      await HandleConcurrencyException(ex, true);
    }

    return result;
  }

  /// <summary>
  /// Handles concurrency exceptions that occur when updating database records.
  /// </summary>
  /// <param name="ex">The <see cref="DbUpdateConcurrencyException"/> that was thrown.</param>
  /// <param name="isAsync">Indicates whether the operation is asynchronous.</param>
  /// <exception cref="Exception">
  /// Thrown if the record has been deleted by another process or if a concurrency conflict occurs.
  /// </exception>
  /// <exception cref="NotSupportedException">
  /// Thrown if the entity type is not supported for concurrency handling.
  /// </exception>
  /// <remarks>
  /// This method attempts to retrieve the latest database values for the entity involved in the concurrency conflict.
  /// If the entity no longer exists, an exception is thrown. Otherwise, the method updates the original values
  /// of the entity with the database values and raises a concurrency conflict exception with the serialized
  /// database object.
  /// </remarks>
  private static async Task HandleConcurrencyException(
      DbUpdateConcurrencyException ex,
      bool isAsync
  )
  {
    foreach (var entry in ex.Entries)
    {
      if (entry.Entity is TEntity)
      {
        var dbValues = (isAsync == false)
          ? entry.GetDatabaseValues()
          : await entry.GetDatabaseValuesAsync();

        if (dbValues == null)
          throw new Exception("The record has been deleted by another job process.");

        var dbObject = (TEntity)dbValues.ToObject();
        var jsonObject = JsonSerializer.Serialize(dbObject);

        entry.OriginalValues.SetValues(dbValues);

        throw new Exception(
          "Concurrency conflict: The record has been modified by another user. Please try again...",
          ex
        );
      }
      else
      {
        throw new NotSupportedException(
          "Concurrency error. Don't know how " +
          "to handle concurrency conflicts for " + entry.Metadata.Name +
          "Please try again...",
          ex
        );
      }
    }

    throw new NotSupportedException(
      "Concurrency error. Please try again...",
      ex
    );
  }

}
