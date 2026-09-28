using BoilerController.Events;
using BoilerController.Models;

namespace BoilerController.Services;

/// <summary>
/// Provides logging operations.
/// </summary>
public interface IEventLogService
{
    /// <summary>
    /// Logs the event to the file.
    /// </summary>
    /// <param name="eventName">Name of the Event</param>
    /// <param name="eventData">Data of the event</param>
    void Log(string eventName, string eventData);

    /// <summary>
    /// Gets all data from the list.
    /// </summary>
    /// <returns>A list of EventLog</returns>
    List<EventLog> GetAll();
}
