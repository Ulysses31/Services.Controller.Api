using System.Xml.Serialization;

namespace Services.Controllers.API.Database.Models
{
  /// <summary>
  /// Represents a weather forecast for a specific date, including temperature in Celsius and Fahrenheit, and a summary description.
  /// </summary>
  public class WeatherForecastDto: BaseEntity
  {
    private int _tempF;

    /// <summary>
    /// Gets or sets the unique identifier for the weather forecast.
    /// The ID is automatically generated when a new instance is created.
    /// </summary>
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the date of the weather forecast.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the temperature in Celsius for the forecasted date.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets the temperature in Fahrenheit for the forecasted date, 
    /// calculated from the temperature in Celsius.
    /// </summary>
    public int TemperatureF { 
      get => _tempF; 
      set => _tempF = 32 + (int)(TemperatureC / 0.5556);
    }

    /// <summary>
    /// Gets or sets a summary description of the weather (e.g., sunny, rainy).
    /// </summary>
    public string? Summary { get; set; }
  }
}
