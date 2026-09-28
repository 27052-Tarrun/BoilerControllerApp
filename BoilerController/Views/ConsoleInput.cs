namespace BoilerController.Views;

/// <summary>
/// Provides methods to get inputs from the console.
/// </summary>
public static class ConsoleInput
{
    /// <summary>
    /// Gets integer input in the limit
    /// </summary>
    /// <param name="prompt">Prompt shown to the user</param>
    /// <param name="min">The minimum value accepted</param>
    /// <param name="max">The maximum value accepted</param>
    /// <param name="errorMessage">The error message shown when the conditions not met</param>
    /// <returns></returns>
    public static int ReadIntInRange(string prompt, int min, int max, string errorMessage)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }
    }
}
