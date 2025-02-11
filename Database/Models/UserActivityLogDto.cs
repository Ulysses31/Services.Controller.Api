using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Services.Controllers.API.Database.Models
{
  /// <summary>
  /// Represents a log entry for user activity within the system.
  /// </summary>
  public class UserActivityLogDto : BaseEntity
  {
    /// <summary>
    /// Gets or sets the unique identifier for the log entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the source name of the request.
    /// </summary>
    public string? SourceName { get; set; }

    /// <summary>
    /// Gets or sets the operating system version of the client making the request.
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    /// Gets or sets the hostname of the client.
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// Gets or sets the username of the user making the request.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the domain name associated with the request.
    /// </summary>
    public string? DomainName { get; set; }

    /// <summary>
    /// Gets or sets the IP address of the client making the request.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the HTTP method used in the request (e.g., GET, POST).
    /// </summary>
    public string? RequestMethod { get; set; }

    /// <summary>
    /// Gets or sets the path of the requested resource.
    /// </summary>
    public string? RequestPath { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of when the request was made.
    /// </summary>
    public string? RequestTime { get; set; }

    /// <summary>
    /// Gets or sets the body content of the request.
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// Gets or sets the headers included in the request.
    /// </summary>
    public string? RequestHeaders { get; set; }

    /// <summary>
    /// Gets or sets the headers included in the response.
    /// </summary>
    public string? ResponseHeaders { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code of the response.
    /// </summary>
    public string? ResponseStatusCode { get; set; }

    /// <summary>
    /// Gets or sets the body content of the response.
    /// </summary>
    public string? ResponseBody { get; set; }
  }
}
