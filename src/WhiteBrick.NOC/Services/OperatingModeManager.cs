using WhiteBrick.NOC.Models;

namespace WhiteBrick.NOC.Services;

public sealed class OperatingModeManager
{
    public NocOperatingMode CurrentMode { get; private set; } = NocOperatingMode.Operations;

    public string CurrentModeName => CurrentMode switch
    {
        NocOperatingMode.Operations => "OPERATIONS",
        NocOperatingMode.Investigation => "INVESTIGATION",
        NocOperatingMode.Alert => "ALERT",
        NocOperatingMode.Night => "NIGHT",
        _ => "OPERATIONS"
    };

    public string MoodColor => CurrentMode switch
    {
        NocOperatingMode.Operations => "#2436D7FF",
        NocOperatingMode.Investigation => "#245B7DFF",
        NocOperatingMode.Alert => "#38FF5050",
        NocOperatingMode.Night => "#180C1840",
        _ => "#2436D7FF"
    };

    public string BannerText => CurrentMode switch
    {
        NocOperatingMode.Operations => "OPERATIONS MODE // ALL SYSTEMS NOMINAL",
        NocOperatingMode.Investigation => "INVESTIGATION MODE // DEVICE FOCUS READY",
        NocOperatingMode.Alert => "ALERT MODE // SIMULATED INCIDENT CHANNEL ARMED",
        NocOperatingMode.Night => "NIGHT MODE // LOW-LIGHT OPERATIONS",
        _ => "OPERATIONS MODE"
    };

    public string BannerSeverity => CurrentMode switch
    {
        NocOperatingMode.Alert => "CRITICAL",
        NocOperatingMode.Investigation => "INFO",
        NocOperatingMode.Night => "QUIET",
        _ => "SUCCESS"
    };

    public void CycleMode()
    {
        CurrentMode = CurrentMode switch
        {
            NocOperatingMode.Operations => NocOperatingMode.Investigation,
            NocOperatingMode.Investigation => NocOperatingMode.Alert,
            NocOperatingMode.Alert => NocOperatingMode.Night,
            NocOperatingMode.Night => NocOperatingMode.Operations,
            _ => NocOperatingMode.Operations
        };
    }
}
