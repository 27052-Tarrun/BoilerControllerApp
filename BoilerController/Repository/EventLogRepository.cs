using BoilerController.Models;
using BoilerController.Storage;

namespace BoilerController.Repository;

/// <summary>
/// Manages boiler event log persistence.
/// </summary>
public class EventLogRepository : IEventLogRepository
{
    private readonly IStorage _storage;

    public EventLogRepository(IStorage storage)
    {
        _storage = storage;
    }

    /// <inheritdoc/>
    public void Add(EventLog log)
    {
        _storage.Append(log);
    }

    /// <inheritdoc/>
    public List<EventLog> GetAll()
    {
        return _storage.Load();
    }
}
