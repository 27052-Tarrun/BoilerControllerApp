using BoilerController.Constants;
using BoilerController.Exceptions;
using BoilerController.Models;

namespace BoilerController.Storage;

public class CsvStorage : IStorage
{
    public List<EventLog> Load()
    {
        List<EventLog> logs = [];

        try
        {
            if (!File.Exists(Configurables.LogFilePath))
            {
                return logs;
            }

            string[] lines = File.ReadAllLines(Configurables.LogFilePath);
            foreach (string line in lines.Skip(1))
            {
                string[] values = line.Split(',');
                if (values.Length < 3)
                {
                    continue;
                }

                logs.Add(
                    new EventLog
                    {
                        Timestamp = DateTime.Parse(values[0]),
                        Event = values[1],
                        EventData = values[2]
                    });
            }

            return logs;
        }
        catch (Exception ex)
        {
            throw new StorageException("Failed to load log file.", ex);
        }
    }

    public void Append(EventLog log)
    {
        try
        {
            bool fileExists = File.Exists(Configurables.LogFilePath);
            using StreamWriter writer = new(Configurables.LogFilePath, true);
            if (!fileExists)
            {
                writer.WriteLine("Timestamp,Event,EventData");
            }

            writer.WriteLine($"{log.Timestamp}," + $"{log.Event}," + $"{log.EventData}");
        }
        catch (Exception ex)
        {
            throw new StorageException("Failed to append log file.", ex);
        }
    }
}
