using BoilerController.Models;
using BoilerController.Repository;

namespace BoilerController.Services;

/// <summary>
/// Provides logging operations.
/// </summary>
public class EventLogService : IEventLogService
{
    private readonly IEventLogRepository _repository;

    public EventLogService(IEventLogRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public void Log(string eventName, string eventData)
    {
        EventLog log =
            new()
            {
                Timestamp = DateTime.Now,
                Event = eventName,
                EventData = eventData
            };

        _repository.Add(log);
    }

    /// <inheritdoc/>
    public List<EventLog> GetAll()
    {
        return _repository.GetAll();
    }
}
