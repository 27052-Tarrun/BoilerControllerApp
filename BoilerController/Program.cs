using BoilerController.Controller;
using BoilerController.Repository;
using BoilerController.Services;
using BoilerController.Storage;

namespace BoilerController;

internal class Program
{
    /// <summary>
    /// The application's main entry point.
    /// </summary>
    /// <param name="args">The arguments.</param>
    static void Main(string[] args)
    {
        try
        {
            IStorage storage = new CsvStorage();

            IEventLogRepository repository = new EventLogRepository(storage);

            IEventLogService eventLogService = new EventLogService(repository);

            IBoilerService boilerService = new BoilerService(eventLogService);

            IBoilerController controller = new Boiler_Controller(boilerService, eventLogService);
            controller.Start();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            Console.ResetColor();
        }
    }
}
