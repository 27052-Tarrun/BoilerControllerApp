using BoilerController.Models;

namespace BoilerController.Views;

/// <summary>
/// Provides methods to show the Event logs.
/// </summary>
public class EventLogView
{
    private readonly DashboardRenderer?  _renderer;

    public EventLogView(DashboardRenderer? renderer)
    {
        _renderer = renderer;
    }

    /// <summary>
    /// Prints the Logs on the screen
    /// </summary>
    /// <param name="logs">The List of EventLogs</param>
    public void DisplayLogs(List<EventLog> logs)
    {
        _renderer?.ClearContentArea();
        _renderer?.MoveToContentArea();

        Console.WriteLine();
        Console.WriteLine("--- Event Logs ---");
        Console.WriteLine();

        if (logs.Count == 0)
        {
            Console.WriteLine("No logs available.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Timestamp".PadRight(22) + "Event".PadRight(30) + "Data");
            Console.WriteLine(new string('-', 80));
            foreach (EventLog log in logs)
            {
                Console.WriteLine($"{log.Timestamp:dd-MM-yyyy HH:mm:ss}".PadRight(22) + $"{log.Event}".PadRight(30) + $"{log.EventData}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter To Continue...");
        Console.ReadLine();
    }
}