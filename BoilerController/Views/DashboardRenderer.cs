using BoilerController.Enums;
using BoilerController.Models;

namespace BoilerController.Views;

/// <summary>
/// Renders boiler dashboard.
/// </summary>
public class DashboardRenderer
{
    private const int DashboardHeight = 10;
    private BoilerModel _boiler;

    public DashboardRenderer(BoilerModel boiler)
    {
        _boiler = boiler;
    }

    public int ContentStartLine => DashboardHeight + 1;

    public void UpdateBoiler(BoilerModel boiler)
    {
        _boiler = boiler;
    }

    public void RenderDashboard()
    {
        try
        {
            int currentTop = Console.CursorTop;
            int currentLeft = Console.CursorLeft;

            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < DashboardHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, 0);
            DisplayHeader();
            DisplayStatus();

            if (currentTop >= ContentStartLine)
            {
                Console.SetCursorPosition(currentLeft, currentTop);
            }
        }
        catch
        {
            Console.Clear();
            DisplayHeader();
            DisplayStatus();
        }
    }

    public void ClearContentArea()
    {
        for (int line = ContentStartLine; line < Console.WindowHeight; line++)
        {
            Console.SetCursorPosition(0, line);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        Console.SetCursorPosition(0, ContentStartLine);
    }

    public void MoveToContentArea()
    {
        Console.SetCursorPosition(0, ContentStartLine);
    }

    public void ForceFullRedraw()
    {
        Console.Clear();
        RenderDashboard();
        MoveToContentArea();
    }

    private static void DisplayHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("+============================================================+");
        Console.WriteLine("|                  BOILER CONTROLLER SYSTEM                  |");
        Console.WriteLine("+============================================================+");

        Console.ResetColor();
    }

    private void DisplayStatus()
    {
        Console.Write("Status : ");
        Console.ForegroundColor =
            _boiler.Status switch
            {
                BoilerStatus.Lockout => ConsoleColor.Red,
                BoilerStatus.Ready => ConsoleColor.Yellow,
                BoilerStatus.Running => ConsoleColor.Green,
                BoilerStatus.Operational => ConsoleColor.Green,
                _ => ConsoleColor.White
            };
        Console.WriteLine(_boiler.Status);
        Console.ResetColor();

        Console.WriteLine( $"Interlock : {_boiler.InterlockState}");
        if (_boiler.CurrentCycle != null)
        {
            DisplayProgress(_boiler.CurrentCycle);
        }
    }

    private static void DisplayProgress(BoilerCycle cycle)
    {
        Console.WriteLine($"Phase : {cycle.Phase}");

        int width = 40;
        int filled = (int)(cycle.ProgressPercentage / 100 * width);
        int remaining = width - filled;

        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('#', filled));

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(new string('-', remaining));
        Console.ResetColor();

        Console.WriteLine($"] {cycle.ProgressPercentage:F1}%");
        Console.WriteLine($"Remaining : {cycle.RemainingSeconds}s");
    }
}