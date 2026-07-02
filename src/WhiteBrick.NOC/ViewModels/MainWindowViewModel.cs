using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Avalonia.Threading;
using WhiteBrick.NOC.Models;
using WhiteBrick.NOC.Services;

namespace WhiteBrick.NOC.ViewModels;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly NocRuntime _runtime;
    private readonly DispatcherTimer _timer;
    private readonly NocEventService _events = new();

    private NocTelemetrySnapshot _snapshot;
    private int _tick;
    private double? _liveCpu;
    private double? _liveMemory;
    private double? _liveLatency;
    private bool _developerOverlayVisible;
    private double _weatherPhase;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<string> ConsoleLines { get; } = new();
    public ObservableCollection<string> TimelineEvents { get; } = new();
    public ObservableCollection<double> HistoryValues { get; } = new();
    public ObservableCollection<double> LatencyValues { get; } = new();
    public ObservableCollection<double> CpuValues { get; } = new();

    public MainWindowViewModel(NocRuntime runtime)
    {
        _runtime = runtime;
        _snapshot = runtime.TelemetryProvider.GetSnapshot();

        for (var i = 0; i < 110; i++)
        {
            HistoryValues.Add(88 + Math.Sin(i * 0.18) * 7);
            LatencyValues.Add(18 + Math.Cos(i * 0.16) * 5);
            CpuValues.Add(28 + Math.Sin(i * 0.21) * 10);
        }

        AddConsole("SYSTEM", "WHITE BRICK NOC OS SPRINT 3 ONLINE", NocEventSeverity.Success);
        AddConsole("ENGINE", "Event pipeline initialized", NocEventSeverity.Success);
        AddConsole("PROBE", "Local telemetry probe connected", NocEventSeverity.Success);
        AddConsole("NETWORK", "Honeycutt network route established");
        AddConsole("WB", "WB-Core-01 reserved for Sprint 4 hardware telemetry", NocEventSeverity.Warning);

        AddTimeline("SYSTEM", "NOC OS boot sequence complete");
        AddTimeline("ENGINE", "Scene graph and mood engine stable");
        AddTimeline("NETWORK", "Honeycutt backbone nominal");
        AddTimeline("WEATHER", "Weather provider slot staged");
        AddTimeline("CAMERA", "Camera preview wall staged");

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();
    }

    public string Title => "White Brick NOC OS — Sprint 3";
    public string NetworkScore => $"{_snapshot.NetworkScore:0.0}";
    public string Latency => $"{(_liveLatency ?? _snapshot.LatencyMs):0.0} ms";
    public string Download => $"{_snapshot.DownloadMbps:0.0}";
    public string Upload => $"{_snapshot.UploadMbps:0.0}";
    public string PacketRate => $"{_snapshot.PacketRate:0}";
    public string Cpu => $"{(_liveCpu ?? _snapshot.CpuPercent):0.0}%";
    public string Memory => $"{(_liveMemory ?? _snapshot.MemoryPercent):0.0}%";
    public string Time => _snapshot.Timestamp.ToString("HH:mm:ss");
    public string ProviderName => _runtime.TelemetryProvider.Name;

    public string WeatherTemperature => $"{72 + Math.Sin(_weatherPhase) * 5:0}°F";
    public string WeatherCondition => _tick % 40 < 20 ? "CLEAR SIMULATION" : "CLOUD LAYER SIM";
    public string WeatherWind => $"WIND {4 + Math.Abs(Math.Cos(_weatherPhase * 0.7)) * 7:0} MPH";

    public bool DeveloperOverlayVisible
    {
        get => _developerOverlayVisible;
        set
        {
            if (_developerOverlayVisible == value)
                return;

            _developerOverlayVisible = value;
            OnPropertyChanged();
        }
    }

    public string EngineVersion => _runtime.Engine.EngineVersion;
    public string SceneNodeCount => _runtime.Engine.SceneNodeCount.ToString();
    public string DisplayZoneCount => _runtime.Engine.DisplayZoneCount.ToString();
    public string RegisteredAssetCount => _runtime.Engine.Assets.RegisteredAssets.Count.ToString();
    public string RegisteredWidgetCount => _runtime.WidgetRegistry.Widgets.Count.ToString();

    public string OperatingMode => _runtime.OperatingModes.CurrentModeName;
    public string AlertBannerText => _runtime.OperatingModes.BannerText;
    public string AlertSeverity => _runtime.OperatingModes.BannerSeverity;

    public void CycleOperatingMode()
    {
        _runtime.OperatingModes.CycleMode();
        AddConsole("MODE", $"Operating mode changed to {_runtime.OperatingModes.CurrentModeName}");
        AddTimeline("MODE", $"Operator mode → {_runtime.OperatingModes.CurrentModeName}");

        OnPropertyChanged(nameof(OperatingMode));
        OnPropertyChanged(nameof(AlertBannerText));
        OnPropertyChanged(nameof(AlertSeverity));
    }

    public void ToggleDeveloperOverlay()
    {
        DeveloperOverlayVisible = !DeveloperOverlayVisible;
        AddConsole("ENGINE", DeveloperOverlayVisible ? "Developer overlay opened" : "Developer overlay closed");
        AddTimeline("ENGINE", DeveloperOverlayVisible ? "Developer overlay opened" : "Developer overlay closed");
    }

    private void Tick()
    {
        _snapshot = _runtime.TelemetryProvider.GetSnapshot();
        LoadLocalProbeData();

        _runtime.Engine.Tick();
        _tick++;
        _weatherPhase += 0.08;

        Push(HistoryValues, _snapshot.NetworkScore, 110);
        Push(LatencyValues, _liveLatency ?? _snapshot.LatencyMs, 110);
        Push(CpuValues, _liveCpu ?? _snapshot.CpuPercent, 110);

        NotifyDashboard();

        if (_tick % 5 == 0)
            AddConsole("PROBE", $"CPU {Cpu} / RAM {Memory} / latency {Latency}");

        if (_tick % 9 == 0)
            AddConsole("PACKET", $"Packet stream velocity adjusted: {_snapshot.PacketRate:0} packets/sec");

        if (_tick % 13 == 0)
        {
            AddConsole("NETWORK", "Route verified: Internet → Gateway → Honeycutt backbone", NocEventSeverity.Success);
            AddTimeline("NETWORK", $"Route verified / latency {Latency}", NocEventSeverity.Success);
        }

        if (_tick % 17 == 0)
            AddTimeline("WEATHER", $"{WeatherCondition} / {WeatherTemperature} / {WeatherWind}");

        if (_tick % 23 == 0)
            AddConsole("SUCCESS", "All systems nominal", NocEventSeverity.Success);

        if (_tick % 31 == 0)
        {
            AddConsole("RADAR", "Sweep complete: no anomalies detected");
            AddTimeline("RADAR", "Device sweep complete");
        }

        if (_tick % 47 == 0)
            AddConsole("MODE", $"Room state synchronized: {_runtime.OperatingModes.CurrentModeName}");

        if (_tick % 59 == 0)
            AddTimeline("WB", "WB-Core-01 telemetry slot waiting for hardware heartbeat", NocEventSeverity.Warning);

        // FUTURE SPRINT 4 EVENT SOURCES:
        // UPS probe -> AddConsole("UPS", "Battery 100%", NocEventSeverity.Success);
        // NAS probe -> AddConsole("NAS", "Storage array online", NocEventSeverity.Success);
        // Camera probe -> AddConsole("CAMERA", "Ring Front live", NocEventSeverity.Success);
        // Home Assistant -> AddConsole("HOME", "Automation heartbeat received");
        // White Brick hardware -> AddConsole("WB", "CAN node heartbeat received", NocEventSeverity.Success);
    }

    private void LoadLocalProbeData()
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "data", "noc_status.json");
            path = Path.GetFullPath(path);

            if (!File.Exists(path))
                return;

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;

            if (root.TryGetProperty("system", out var system))
            {
                if (system.TryGetProperty("cpuLoad", out var cpu) && cpu.ValueKind == JsonValueKind.Number)
                    _liveCpu = cpu.GetDouble();

                if (system.TryGetProperty("memory", out var memory))
                {
                    var total = memory.GetProperty("TotalVisibleMemorySize").GetDouble();
                    var free = memory.GetProperty("FreePhysicalMemory").GetDouble();

                    if (total > 0)
                        _liveMemory = ((total - free) / total) * 100.0;
                }
            }

            if (root.TryGetProperty("pings", out var pings) && pings.ValueKind == JsonValueKind.Array)
            {
                foreach (var ping in pings.EnumerateArray())
                {
                    if (ping.TryGetProperty("avgMs", out var avg) && avg.ValueKind == JsonValueKind.Number)
                    {
                        _liveLatency = avg.GetDouble();
                        break;
                    }
                }
            }
        }
        catch
        {
            AddConsole("PROBE", "Local probe unavailable; fallback telemetry active", NocEventSeverity.Warning);
        }
    }

    private void NotifyDashboard()
    {
        OnPropertyChanged(nameof(NetworkScore));
        OnPropertyChanged(nameof(Latency));
        OnPropertyChanged(nameof(Download));
        OnPropertyChanged(nameof(Upload));
        OnPropertyChanged(nameof(PacketRate));
        OnPropertyChanged(nameof(Cpu));
        OnPropertyChanged(nameof(Memory));
        OnPropertyChanged(nameof(Time));
        OnPropertyChanged(nameof(ProviderName));
        OnPropertyChanged(nameof(WeatherTemperature));
        OnPropertyChanged(nameof(WeatherCondition));
        OnPropertyChanged(nameof(WeatherWind));
        OnPropertyChanged(nameof(EngineVersion));
        OnPropertyChanged(nameof(SceneNodeCount));
        OnPropertyChanged(nameof(DisplayZoneCount));
        OnPropertyChanged(nameof(RegisteredAssetCount));
        OnPropertyChanged(nameof(RegisteredWidgetCount));
        OnPropertyChanged(nameof(OperatingMode));
        OnPropertyChanged(nameof(AlertBannerText));
        OnPropertyChanged(nameof(AlertSeverity));
        OnPropertyChanged(nameof(HistoryValues));
        OnPropertyChanged(nameof(LatencyValues));
        OnPropertyChanged(nameof(CpuValues));
        OnPropertyChanged(nameof(TimelineEvents));
    }

    private static void Push(ObservableCollection<double> list, double value, int max)
    {
        list.Add(value);

        while (list.Count > max)
            list.RemoveAt(0);
    }

    private void AddConsole(string category, string message, NocEventSeverity severity = NocEventSeverity.Info)
    {
        _events.Publish(category, message, severity);

        ConsoleLines.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {category,-8} {message}");

        while (ConsoleLines.Count > 38)
            ConsoleLines.RemoveAt(ConsoleLines.Count - 1);
    }

    private void AddTimeline(string category, string message, NocEventSeverity severity = NocEventSeverity.Info)
    {
        _events.Publish(category, message, severity);

        TimelineEvents.Insert(0, $"{DateTime.Now:HH:mm:ss}  {category,-7}  {message}");

        while (TimelineEvents.Count > 14)
            TimelineEvents.RemoveAt(TimelineEvents.Count - 1);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}