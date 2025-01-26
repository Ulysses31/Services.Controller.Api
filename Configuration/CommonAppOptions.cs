namespace Services.Controllers.API.Configuration;

/// <summary>
/// CommonAppOptions class
/// </summary>
public class CommonAppOptions
{
  public const string AppOptions = "ApplicationSettings";

  public static string? AppName { get; set; }
  public static string? AppVersion { get; set; }
  public static string? AppDescription { get; set; }
  public static string? AppUrl { get; set; }
  public static bool EnableRateLimiting { get; set; }
  public static bool EnableHealthCheck { get; set; }
  public static bool EnableApiCache { get; set; }
  public static bool EnableGenApiClient { get; set; }

}
