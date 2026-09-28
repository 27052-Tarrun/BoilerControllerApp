using BoilerController.Enums;

namespace BoilerController.Models;

public class BoilerCycle
{
    public DateTime StartTime { get; set; }

    public BoilerPhase Phase { get; set; }

    public double ProgressPercentage { get; set; }

    public int RemainingSeconds { get; set; }
}
