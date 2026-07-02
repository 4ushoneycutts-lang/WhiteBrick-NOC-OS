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
            // create a function to swap windows so we never instantiate both at once
            void ShowCalibration()
            {
                var old = desktop.MainWindow;
                var calib = new Views.CalibrationWindow();
                desktop.MainWindow = calib;
                calib.Show();
                old?.Close();
            }

            void ShowDashboard()
            {
                var old = desktop.MainWindow;
                var runtime = NocRuntime.CreateDefault();
                var main = new MainWindow
                {
                    DataContext = new MainWindowViewModel(runtime)
                };
                desktop.MainWindow = main;
                main.Show();
                old?.Close();
            }

            // TEMPORARILY force Calibration Mode for testing
            ShowCalibration();

            // Attach a global toggle: F10 switches modes at runtime
            if (desktop.MainWindow != null)
            {
                desktop.MainWindow.KeyDown += (s, e) =>
                {
                    try
                    {
                        if (e.Key == Avalonia.Input.Key.F10)
                        {
                            if (desktop.MainWindow is Views.CalibrationWindow)
                                ShowDashboard();
                            else
                                ShowCalibration();
                        }
                    }
                    catch { }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}
