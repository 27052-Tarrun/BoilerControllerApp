using BoilerController.Models;
using BoilerController.Repository;

namespace BoilerController.Services;

public class EventLogService : IEventLogService
{
    private readonly IEventLogRepository _repository;

    public EventLogService(IEventLogRepository repository)
    {
        _repository = repository;
    }

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

    public List<EventLog> GetAll()
    {
        return _repository.GetAll();
    }
}
