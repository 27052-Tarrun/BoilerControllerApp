using BoilerController.Models;

namespace BoilerController.Services;

public interface IEventLogService
{
    void Log(string eventName, string eventData);

    List<EventLog> GetAll();
}
