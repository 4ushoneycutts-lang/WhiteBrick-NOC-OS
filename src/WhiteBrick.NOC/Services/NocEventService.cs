using System.Collections.ObjectModel;

namespace WhiteBrick.NOC.Services;

public sealed class NocEventService
{
    public ObservableCollection<NocEvent> Events { get; } = new();

    public void Publish(
        string source,
        string message,
        NocEventSeverity severity = NocEventSeverity.Info)
    {
        Events.Insert(0, new NocEvent(DateTime.Now, source, message, severity));

        while (Events.Count > 250)
            Events.RemoveAt(Events.Count - 1);
    }
}