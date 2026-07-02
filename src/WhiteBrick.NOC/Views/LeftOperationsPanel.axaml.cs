using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WhiteBrick.NOC.Views
{
    public partial class LeftOperationsPanel : UserControl
    {
        public LeftOperationsPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
