using BoilerController.Models;

namespace BoilerController.Storage;

public interface IStorage
{
    List<EventLog> Load();

    void Append(EventLog log);
}