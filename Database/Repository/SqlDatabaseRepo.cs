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

  #region Sync Methods

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

  #endregion Sync Methods

  #region Async Methods

  /// <inheritdoc/>
  public override async Task<PagedResult<TEntity>> FilterPaginationAsync(PaginationQuery paginationQuery)
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
  public override async Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate)
  {
    return await Task.FromResult(_context.Set<TEntity>().Where(predicate));
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
    var entityToUpdate = (await FilterAsNoTrackingAsync(predicate)).FirstOrDefault() ?? throw new Exception("Entity not found.");
    entityToUpdate = entity ?? throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    _context.Entry(entity).State = EntityState.Modified;
    await SaveChangesAsync(_context);
    return entityToUpdate;
  }

  /// <inheritdoc/>
  public override async Task<TEntity> DeleteAsync(TEntity entity)
  {
    if (entity == null) throw new ArgumentNullException(NullEntity, nameof(NullEntity));

    _context.Set<TEntity>().Remove(entity);
    await SaveChangesAsync(_context);
    return entity;
  }

  #endregion Async Methods

  /// <inheritdoc/>
  public override int SaveChanges(DbContext context)
  {
    return context.SaveChanges();
  }

  /// <inheritdoc/>
  public override async Task<int> SaveChangesAsync(DbContext context)
  {
    return await context.SaveChangesAsync();
  }




}
