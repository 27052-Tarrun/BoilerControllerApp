namespace BoilerController.Models;

public class EventLog
{
    public DateTime Timestamp { get; set; }

    public string Event { get; set; } = string.Empty;

    public string EventData { get; set; } = string.Empty;
}
