namespace Services.Controllers.API.Configuration
{
  /// <summary>
  /// Represents supported database types as constant string properties.
  /// </summary>
  public class DbTypeEnum
  {
    /// <summary>
    /// Represents Microsoft SQL Server database type.
    /// </summary>
    public static string MsSql { get; } = "MsSql";

    /// <summary>
    /// Represents SQLite database type.
    /// </summary>
    public static string SqLite { get; } = "SqLite";

    /// <summary>
    /// Represents MySQL database type.
    /// </summary>
    public static string MySql { get; } = "MySql";

    /// <summary>
    /// Represents MongoDB database type.
    /// </summary>
    public static string MongoDb { get; } = "MongoDb";
  }
}
