using BoilerController.Enums;

namespace BoilerController.Models;

public class BoilerModel
{
    public BoilerStatus Status { get; set; }

    public InterlockState InterlockState { get; set; }

    public BoilerCycle? CurrentCycle { get; set; }
}
