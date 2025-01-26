namespace Services.Controllers.API.Configuration
{
  /// <summary>
  /// Represents configuration options for the application.
  /// This class provides constants and static properties for managing application settings.
  /// </summary>
  public class CommonAppOptions
  {
    /// <summary>
    /// The name of the application settings section in the configuration file.
    /// </summary>
    public const string AppOptions = "ApplicationSettings";

    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public static string? AppName { get; set; }

    /// <summary>
    /// Gets or sets the version of the application.
    /// </summary>
    public static string? AppVersion { get; set; }

    /// <summary>
    /// Gets or sets the description of the application.
    /// </summary>
    public static string? AppDescription { get; set; }

    /// <summary>
    /// Gets or sets the URL of the application.
    /// </summary>
    public static string? AppUrl { get; set; }

    /// <summary>
    /// Indicates whether rate limiting is enabled for the application.
    /// </summary>
    public static bool EnableRateLimiting { get; set; }

    /// <summary>
    /// Indicates whether health checks are enabled for the application.
    /// </summary>
    public static bool EnableHealthCheck { get; set; }

    /// <summary>
    /// Indicates whether API caching is enabled for the application.
    /// </summary>
    public static bool EnableApiCache { get; set; }

    /// <summary>
    /// Indicates whether the generation of an API client is enabled for the application.
    /// </summary>
    public static bool EnableGenApiClient { get; set; }
  }
}
