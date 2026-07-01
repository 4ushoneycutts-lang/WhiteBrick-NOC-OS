using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WhiteBrick.NOC.Services;
using WhiteBrick.NOC.ViewModels;
using WhiteBrick.NOC.Views;

namespace WhiteBrick.NOC;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var runtime = NocRuntime.CreateDefault();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(runtime)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
