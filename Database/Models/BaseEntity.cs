using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Controllers.API.Database.Models;

/// <summary>
/// Represents the base entity for database models, providing common properties.
/// </summary>
public class BaseEntity
{
  /// <summary>
  /// Gets a value indicating whether there is a previous page version. 
  /// </summary>
  public Guid Version { get; set; }

  /// <summary>
  /// Gets or sets the user who created the record.
  /// </summary>
  public string? CreatedBy { get; set; }

  /// <summary>
  /// Gets or sets the date and time the record was created.
  /// </summary>
  public DateTime CreatedDate { get; set; }

  /// <summary>
  /// Gets or sets the date and time the record was last updated.
  /// </summary>
  public DateTime? ModifiedDate { get; set; }
}
