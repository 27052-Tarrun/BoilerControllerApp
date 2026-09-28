using BoilerController.Models;

namespace BoilerController.Storage;

/// <summary>
/// Provides methods to communicate with the file.
/// </summary>
public interface IStorage
{
    /// <summary>
    /// Loads the data from the file.
    /// </summary>
    /// <returns>List of EventLog</returns>
    List<EventLog> Load();

    /// <summary>
    /// Append the EventLog to the file.
    /// </summary>
    /// <param name="log">A EventLog object</param>
    void Append(EventLog log);
}
