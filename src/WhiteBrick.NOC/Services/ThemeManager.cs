namespace WhiteBrick.NOC.Services;

public sealed class ThemeManager
{
    public string ThemeName { get; private set; } = "WhiteBrick.Dark.Cinematic";
    public string AccentPrimary { get; private set; } = "#24D7FF";
    public string AccentHealthy { get; private set; } = "#6EFFA1";
    public string AccentWarning { get; private set; } = "#FFC857";
    public string AccentCritical { get; private set; } = "#FF5050";

    public void UseCinematicDark()
    {
        ThemeName = "WhiteBrick.Dark.Cinematic";
        AccentPrimary = "#24D7FF";
        AccentHealthy = "#6EFFA1";
        AccentWarning = "#FFC857";
        AccentCritical = "#FF5050";
    }
}
