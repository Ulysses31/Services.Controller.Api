using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Services.Controllers.API.Models
{
  /// <summary>
  /// Represents a weather forecast for a specific date, including temperature in Celsius and Fahrenheit, and a summary description.
  /// </summary>
  public class WeatherForecastResponse
  {
    /// <summary>
    /// Gets or sets the unique identifier for the weather forecast.
    /// The ID is automatically generated when a new instance is created.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the date of the weather forecast.
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the temperature in Celsius for the forecasted date.
    /// </summary>
    [JsonPropertyName("temperatureC")]
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets the temperature in Fahrenheit for the forecasted date, 
    /// calculated from the temperature in Celsius.
    /// </summary>
    [JsonPropertyName("temperatureF")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Gets or sets a summary description of the weather (e.g., sunny, rainy).
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
  }
}
