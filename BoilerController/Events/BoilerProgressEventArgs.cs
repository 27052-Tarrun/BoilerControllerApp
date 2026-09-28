using BoilerController.Enums;

namespace BoilerController.Events;

public class BoilerProgressEventArgs : EventArgs
{
    public BoilerPhase Phase { get; set; }

    public double ProgressPercentage { get; set; }

    public int RemainingSeconds { get; set; }
}
