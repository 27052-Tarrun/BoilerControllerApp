using BoilerController.Enums;

namespace BoilerController.Models;

/// <summary>
/// Represents the Event Log model.
/// </summary>
public class EventLog
{
    /// <summary>
    /// Gets or sets the time.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the event name.
    /// </summary>
    public string Event { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event data.
    /// </summary>
    public string EventData { get; set; } = string.Empty;
}
