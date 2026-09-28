using BoilerController.Models;
using BoilerController.Storage;

namespace BoilerController.Repository;

public class EventLogRepository : IEventLogRepository
{
    private readonly IStorage _storage;

    public EventLogRepository(IStorage storage)
    {
        _storage = storage;
    }

    public void Add(EventLog log)
    {
        _storage.Append(log);
    }

    public List<EventLog> GetAll()
    {
        return _storage.Load();
    }
}
