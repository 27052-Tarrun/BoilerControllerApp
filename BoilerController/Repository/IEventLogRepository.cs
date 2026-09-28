using BoilerController.Models;

namespace BoilerController.Repository;

public interface IEventLogRepository
{
    void Add(EventLog log);

    List<EventLog> GetAll();
}
