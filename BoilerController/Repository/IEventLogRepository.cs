using BoilerController.Models;

namespace BoilerController.Repository;

/// <summary>
/// Manages boiler event log persistence.
/// </summary>
public interface IEventLogRepository
{
    /// <summary>
    /// Adds a log entry.
    /// </summary>
    /// <param name="log">Event log to persist.</param>
    void Add(EventLog log);

    /// <summary>
    /// Retrieves all event logs.
    /// </summary>
    /// <returns>Collection of event logs.</returns>
    List<EventLog> GetAll();
}
