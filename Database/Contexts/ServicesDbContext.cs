using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Controllers.API.Database.Models;

namespace Services.Controllers.API.Database.Contexts
{
  /// <summary>
  /// Represents the database context for the application, providing access to the database and managing entity changes.
  /// </summary>
  public class ServicesDbContext : DbContext
  {
    /// <summary>
    /// Logger for capturing information about the database context's operations.
    /// </summary>
    private readonly ILogger<ServicesDbContext>? _logger;

    /// <summary>
    /// Gets or sets the DbSet for WeatherForecast entities.
    /// </summary>
    public DbSet<WeatherForecastDto> weatherForecasts { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServicesDbContext"/> class with the specified options and logger.
    /// </summary>
    /// <param name="options">Options for configuring the database context.</param>
    /// <param name="logger">Logger for capturing context operations.</param>
    public ServicesDbContext(
        DbContextOptions<ServicesDbContext> options,
        ILogger<ServicesDbContext> logger
    ) : base(options)
    {
      ChangeTracker.StateChanged += UpdateTimestamps;
      ChangeTracker.Tracked += UpdateTimestamps;
      _logger = logger;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServicesDbContext"/> class for testing or derived contexts.
    /// </summary>
    protected ServicesDbContext()
    {
    }

    /// <summary>
    /// Configures the model for the database context. 
    /// Override this method to customize the EF model during model creation.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for the database context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<WeatherForecastDto>(entity =>
      {
        entity.HasKey(key => key.Id)
              .HasName("PK_WeatherForecast_WeatherForecastId");
        entity.HasIndex(key => key.Id, "AK_WeatherForecast_WeatherForecastId")
              .IsUnique();
        entity.ToTable("WeatherForecast", tb =>
        {
          tb.HasComment("A weather forecast for a specific date.");
        });

        entity.Property(key => key.Id)
              .HasComment("The unique identifier for the weather forecast.")
              .HasColumnName("Id")
              .HasColumnType("varchar")
              .HasMaxLength(36)
              .IsRequired();
        //.ValueGeneratedOnAdd();

        entity.Property(key => key.Date)
              .HasComment("The date of the weather forecast.")
              .HasColumnName("Date")
              .HasColumnType("date")
              .IsRequired();

        entity.Property(key => key.TemperatureC)
              .HasComment("The temperature in Celsius for the forecasted date.")
              .HasColumnName("TemperatureC")
              .HasColumnType("int")
              .IsRequired();

        entity.Property(key => key.TemperatureF)
              .HasComment("The temperature in Fahrenheit for the forecasted date, calculated from the temperature in Celsius.")
              .HasColumnName("TemperatureF")
              .HasColumnType("int")
              .IsRequired();

        entity.Property(key => key.Summary)
              .HasComment("A summary description of the weather (e.g., sunny, rainy).")
              .HasColumnName("Summary")
              .HasColumnType("varchar")
              .IsRequired();

        entity.Property(key => key.CreatedBy)
              .HasMaxLength(100)
              .HasComment("Who created the record.")
              .IsRequired(true);

        entity.Property(key => key.CreatedDate)
              .HasComment("Date and time the record was created.")
              .HasColumnType("datetime")
              .IsRequired(true);

        entity.Property(key => key.ModifiedDate)
              .HasComment("Date and time the record was last updated.")
              .HasColumnType("datetime");

        // Initial seed data
        entity.HasData(
          new WeatherForecastDto
          {
            Id = new Guid("c3d61411-9123-4839-ad75-004507d26e85").ToString(),
            Date = new DateTime(2025, 1, 1),
            TemperatureC = 20,
            Summary = "Sunny",
            CreatedBy = "System",
            CreatedDate = new DateTime(2025, 1, 1, 12, 0, 0)
          }
        );
      });
    }

    /// <summary>
    /// Updates timestamps for entities when their state changes or they are being tracked.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Provides data for the entity entry event.</param>
    private void UpdateTimestamps(object? sender, EntityEntryEventArgs e)
    {
      if (e.Entry.Entity is BaseEntity entityWithTimestamps)
      {
        switch (e.Entry.State)
        {
          case EntityState.Modified:
            entityWithTimestamps.ModifiedDate = DateTimeOffset.Now.DateTime;
            _logger!.LogInformation("Stamped for update: {Entity}", e.Entry.Entity);
            break;

          case EntityState.Added:
            entityWithTimestamps.CreatedDate = DateTimeOffset.Now.DateTime;
            _logger!.LogInformation("Stamped for insert: {Entity}", e.Entry.Entity);
            break;
        }
      }
    }
  }
}
