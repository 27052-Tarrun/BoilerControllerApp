using BoilerController.Constants;
using BoilerController.Enums;
using BoilerController.Models;

namespace BoilerController.Views;

public class DashboardView
{
    private DashboardRenderer? _renderer;

    public MenuOption Show(BoilerModel boiler)
    {
        if (_renderer == null)
        {
            _renderer = new DashboardRenderer(boiler);
            Console.Clear();
            _renderer.RenderDashboard();
        }
        else
        {
            _renderer.UpdateBoiler(boiler);
            _renderer.RenderDashboard();
        }

        _renderer.MoveToContentArea();

        DisplayMenu(boiler);
        int choice = ConsoleInput.ReadIntInRange("Choice : ", 0, 4, ErrorMessages.InvalidMenuChoice);
        return (MenuOption)choice;
    }

    public void UpdateDashboard(BoilerModel boiler)
    {
        if (_renderer == null)
        {
            return;
        }

        _renderer.UpdateBoiler(boiler);
        _renderer.RenderDashboard();
    }

    public void EnsureRendererInitialized(BoilerModel boiler)
    {
        if (_renderer == null)
        {
            _renderer = new DashboardRenderer(boiler);
            Console.Clear();
            _renderer.RenderDashboard();
        }
    }

    public DashboardRenderer? GetRenderer()
    {
        return _renderer;
    }

    public void DisplayMessageWithRedraw(BoilerModel boiler, string message)
    {
        ClearContentArea();
        MoveToContentArea();
        Console.WriteLine(message);
        Console.WriteLine("\nPress Enter To Continue...");
        Console.ReadLine();
        ForceFullRedraw(boiler);
    }

    public void DisplayErrorWithRedraw(BoilerModel boiler, string message)
    {
        ClearContentArea();
        MoveToContentArea();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine("\nPress Enter To Continue...");
        Console.ReadLine();
        ForceFullRedraw(boiler);
    }

    public void DisplaySuccessWithRedraw(BoilerModel boiler, string message)
    {
        ClearContentArea();
        MoveToContentArea();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine("\nPress Enter To Continue...");
        Console.ReadLine();
        ForceFullRedraw(boiler);
    }

    public void DisplayError(string message)
    {
        ClearContentArea();
        MoveToContentArea();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.ReadLine();
    }

    public void ClearContentArea()
    {
        _renderer?.ClearContentArea();
    }

    public void MoveToContentArea()
    {
        _renderer?.MoveToContentArea();
    }

    public void ForceFullRedraw(BoilerModel boiler)
    {
        _renderer?.UpdateBoiler(boiler);
        _renderer?.ForceFullRedraw();
    }

    private static void DisplayMenu(BoilerModel boiler)
    {
        Console.WriteLine("+----------------------------------------------------------+");
        Console.WriteLine("| 1. Toggle Run Interlock Switch                           |");
        Console.WriteLine("| 2. Reset Lockout                                         |");
        Console.WriteLine("| 3. Start Boiler Sequence                                 |");
        Console.WriteLine("| 4. Stop Boiler Sequence                                  |");
        Console.WriteLine("| 5. View Event Log                                        |");
        Console.WriteLine("| 0. Exit                                                  |");
        Console.WriteLine("+----------------------------------------------------------+");
    }
}