using System.Windows;
using System.Windows.Controls;

namespace Halcyon.WLED
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public WLED Plugin { get; }

        public SettingsControl()
        {
            InitializeComponent();
        }

        public SettingsControl(WLED plugin) : this()
        {
            this.Plugin = plugin;
            UrlInput.Text = Plugin.Settings.stripUrl;
            PortInput.Text = Plugin.Settings.stripPort.ToString();
            LedAmout.Text = Plugin.Settings.ledAmount.ToString();
            LedOffset.Text = Plugin.Settings.offset.ToString();
            Mirror.IsChecked = Plugin.Settings.mirror;
            Center.IsChecked = Plugin.Settings.center;
        }

        private void Apply(object sender, RoutedEventArgs e)
        {
            SimHub.Logging.Current.Info("Apply Settings");
            Plugin.Settings.stripUrl = UrlInput.Text;
            Plugin.Settings.stripPort = int.Parse(PortInput.Text);
            Plugin.Settings.ledAmount = int.Parse(LedAmout.Text);
            Plugin.Settings.offset = int.Parse(LedOffset.Text);
            Plugin.Settings.mirror = (bool) Mirror.IsChecked;
            Plugin.Settings.center = (bool) Center.IsChecked;
        }
    }
}
