using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public DbSet<WeatherForecastDto> WeatherForecasts { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for UserActivity entities.
    /// </summary>
    public DbSet<UserActivityLogDto> UserActivities { get; set; }

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
      ChangeTracker.DetectingEntityChanges += LogEntityChanges;
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

      modelBuilder.Entity<UserActivityLogDto>(entity =>
      {
        entity.HasKey(key => key.Id)
              .HasName("PK_UserActivity_UserActivityId");
        entity.HasIndex(key => key.Id, "AK_UserActivity_UserActivityId")
              .IsUnique();
        entity.ToTable("UserActivity", tb =>
        {
          tb.HasComment("A log entry for user activity within the system.");
        });

        entity.Property(key => key.Id)
              .HasComment("The unique identifier for the log entry.")
              .HasColumnName("Id")
              .HasColumnType("integer")
              .IsRequired()
              .ValueGeneratedOnAdd();

        entity.Property(key => key.SourceName)
              .HasComment("The source name of the request.")
              .HasColumnName("SourceName")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(100);

        entity.Property(key => key.OsVersion)
              .HasComment("The operating system version of the client making the request.")
              .HasColumnName("OsVersion")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(100);

        entity.Property(key => key.Host)
              .HasComment("The hostname of the client.")
              .HasColumnName("Host")
#if MYSQL 
                  .HasColumnType("text")
#else
                  .HasColumnType("varchar")
#endif
              .HasMaxLength(100);

        entity.Property(key => key.Username)
              .HasComment("The username of the user making the request.")
              .HasColumnName("Username")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(100);

        entity.Property(key => key.DomainName)
              .HasComment("The domain name associated with the request.")
              .HasColumnName("DomainName")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(100);

        entity.Property(key => key.Address)
              .HasComment("The IP address of the client making the request.")
              .HasColumnName("Address")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(100);

        entity.Property(key => key.RequestMethod)
              .HasComment("The HTTP method used in the request (e.g., GET, POST).")
              .HasColumnName("RequestMethod")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(10);

        entity.Property(key => key.RequestPath)
              .HasComment("The path of the requested resource.")
              .HasColumnName("RequestPath")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(100);

        entity.Property(key => key.RequestTime)
              .HasComment("The timestamp of when the request was made.")
              .HasColumnName("RequestTime")
              .HasColumnType("datetime");

        entity.Property(key => key.RequestBody)
              .HasComment("The body content of the request.")
              .HasColumnName("RequestBody")
#if MYSQL 
              .HasColumnType("text");
#else
              .HasColumnType("varchar");
#endif

        entity.Property(key => key.RequestHeaders)
              .HasComment("The headers included in the request.")
              .HasColumnName("RequestHeaders")
#if MYSQL 
              .HasColumnType("text");
#else
              .HasColumnType("varchar");
#endif

        entity.Property(key => key.ResponseHeaders)
              .HasComment("The headers included in the response.")
              .HasColumnName("ResponseHeaders")
#if MYSQL 
              .HasColumnType("text");
#else
              .HasColumnType("varchar");
#endif

        entity.Property(key => key.ResponseStatusCode)
              .HasComment("The HTTP status code of the response.")
              .HasColumnName("ResponseStatusCode")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(10);

        entity.Property(key => key.ResponseBody)
              .HasComment("The body content of the response.")
              .HasColumnName("ResponseBody")
#if MYSQL 
              .HasColumnType("text");
#else
              .HasColumnType("varchar");
#endif

        entity.Property(key => key.Version)
              .HasComment("The version of the data entry.")
              .HasColumnName("RowVersion")
#if MYSQL 
              .HasColumnType("text")
#else              
              .HasColumnType("varchar")
#endif
              .IsConcurrencyToken();

        entity.Property(key => key.CreatedBy)
              .HasMaxLength(100)
              .HasComment("Who created the record.")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif              
              .IsRequired(true);

        entity.Property(key => key.CreatedDate)
              .HasComment("Date and time the record was created.")
              .HasColumnType("datetime")
              .IsRequired(true);

        entity.Property(key => key.ModifiedDate)
              .HasComment("Date and time the record was last updated.")
              .HasColumnType("datetime");
      });

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
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .HasMaxLength(36)
              .IsRequired();
        //.ValueGeneratedOnAdd();

        entity.Property(key => key.Date)
              .HasComment("The date of the weather forecast.")
              .HasColumnName("Date")
              .HasColumnType("date")
              .HasConversion(
                v => v.ToString("yyyy-MM-dd"),
                v => DateOnly.Parse(v)
              )
              .IsRequired();

        entity.Property(key => key.TemperatureC)
              .HasComment("The temperature in Celsius for the forecasted date.")
              .HasColumnName("TemperatureC")
              .HasColumnType("integer")
              .IsRequired();

        entity.Property(key => key.TemperatureF)
              .HasComment("The temperature in Fahrenheit for the forecasted date, calculated from the temperature in Celsius.")
              .HasColumnName("TemperatureF")
              .HasColumnType("integer")
              .IsRequired();

        entity.Property(key => key.Summary)
              .HasComment("A summary description of the weather (e.g., sunny, rainy).")
              .HasColumnName("Summary")
#if MYSQL 
              .HasColumnType("text")
#else
              .HasColumnType("varchar")
#endif
              .IsRequired();

        entity.Property(key => key.Version)
              .HasComment("The version of the data entry.")
              .HasColumnName("RowVersion")
#if MYSQL 
              .HasColumnType("text")  
#else
              .HasColumnType("varchar")
#endif
              .IsConcurrencyToken();

        entity.Property(key => key.CreatedBy)
              .HasMaxLength(100)
              .HasComment("Who created the record.")
#if MYSQL 
              .HasColumnType("text")  
#else
              .HasColumnType("varchar")
#endif
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
            Id = new Guid("38b7942a-8a8f-4a34-9744-e4dea6eaed78").ToString(),
            Date = DateOnly.Parse(new DateTime(2025, 01, 04).ToString("yyyy-MM-dd")),
            TemperatureC = 25,
            TemperatureF = 32 + (int)(25 / 0.5556),
            Summary = "Hot",
            Version = new Guid("38b7942a-8a8f-4a34-9744-e4dea6eaed78"),
            CreatedBy = "System",
            CreatedDate = new DateTime(2025, 01, 04)
          },
          new WeatherForecastDto
          {
            Id = new Guid("3db3a34a-9dcf-42e6-977f-d6bbb2329f16").ToString(),
            Date = DateOnly.Parse(new DateTime(2025, 01, 07).ToString("yyyy-MM-dd")),
            TemperatureC = 15,
            TemperatureF = 32 + (int)(15 / 0.5556),
            Summary = "Cool",
            Version = new Guid("3db3a34a-9dcf-42e6-977f-d6bbb2329f16"),
            CreatedBy = "System",
            CreatedDate = new DateTime(2025, 01, 07)
          },
          new WeatherForecastDto
          {
            Id = new Guid("76d5e039-63b3-4c7f-bb8d-0847f729dcde").ToString(),
            Date = DateOnly.Parse(new DateTime(2025, 01, 09).ToString("yyyy-MM-dd")),
            TemperatureC = 5,
            TemperatureF = 32 + (int)(5 / 0.5556),
            Summary = "Cold",
            Version = new Guid("76d5e039-63b3-4c7f-bb8d-0847f729dcde"),
            CreatedBy = "System",
            CreatedDate = new DateTime(2025, 01, 09)
          },
          new WeatherForecastDto
          {
            Id = new Guid("1130f076-1d75-4977-8a50-323a4ecf8f4e").ToString(),
            Date = DateOnly.Parse(new DateTime(2025, 01, 11).ToString("yyyy-MM-dd")),
            TemperatureC = 35,
            TemperatureF = 32 + (int)(35 / 0.5556),
            Summary = "Hot",
            Version = new Guid("1130f076-1d75-4977-8a50-323a4ecf8f4e"),
            CreatedBy = "System",
            CreatedDate = new DateTime(2025, 01, 11)
          },
          new WeatherForecastDto
          {
            Id = new Guid("2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04").ToString(),
            Date = DateOnly.Parse(new DateTime(2025, 01, 20).ToString("yyyy-MM-dd")),
            TemperatureC = 20,
            TemperatureF = 32 + (int)(20 / 0.5556),
            Summary = "Warm",
            Version = new Guid("2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04"),
            CreatedBy = "System",
            CreatedDate = new DateTime(2025, 01, 20)
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
          case EntityState.Deleted:
            _logger!.LogInformation("===> 🗑️ Stamped for delete: {Entity}", e.Entry.Entity);
            break;
          case EntityState.Modified:
            entityWithTimestamps.ModifiedDate = DateTimeOffset.Now.DateTime;
            entityWithTimestamps.Version = Guid.NewGuid();
            _logger!.LogInformation("===> 📅 Stamped for update: {Entity}", e.Entry.Entity);
            break;
          case EntityState.Added:
            entityWithTimestamps.CreatedDate = DateTimeOffset.Now.DateTime;
            entityWithTimestamps.Version = Guid.NewGuid();
            _logger!.LogInformation("===> 📅 Stamped for insert: {Entity}", e.Entry.Entity);
            break;
        }
      }
    }


    private void LogEntityChanges(
      object? sender,
      DetectEntityChangesEventArgs e
    )
    {
      if ((e.Entry.Entity is BaseEntity) && (e.Entry.Entity is not UserActivityLogDto))
      {
        _logger!.LogInformation("========== Entity Tracking =====================================>");
        _logger!.LogInformation("Entity: {Entity}", e.Entry.Entity);
        _logger!.LogInformation("State: {State}", e.Entry.State);
        _logger!.LogInformation("========== Entity Tracking =====================================>");

        // Get modified properties
        var modifiedProperties = ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Modified)
            .ToList();

        // Log modified fields
        foreach (var entry in modifiedProperties)
        {
          var dbVals = entry.GetDatabaseValues();

          _logger!.LogInformation("========== Modified Entity Properties Tracking =================>");
          foreach (var prop in entry.Properties.Where(p => p.IsModified && !Equals(dbVals![p.Metadata.Name], p.CurrentValue)))
          {
            var tp = prop.Metadata.ClrType;

            _logger!.LogInformation(
              "Property => Type: [{Type}] => Name: [{Property}] => DatabaseValue: [{db}] => NewValue: [{Value}]",
              tp.Name,
              prop.Metadata.Name,
              dbVals![prop.Metadata.Name],
              prop.CurrentValue
            );
          }
          _logger!.LogInformation("========== Modified Entity Properties Tracking =================>");
        }
      }
    }
  }
}
