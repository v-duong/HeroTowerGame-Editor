using Ookii.Dialogs.Wpf;
using System.Windows;


namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class JSONSettingsWindow : Window
    {
        public JSONSettingsWindow()
        {
            InitializeComponent();
            JsonLoadTxt.Text = Properties.Settings.Default.JsonLoadPath;
            JsonSaveTxt.Text = Properties.Settings.Default.JsonSavePath;
        }

        private void BrowseButton(object sender, RoutedEventArgs e)
        {
            var f = new VistaFolderBrowserDialog();
            f.ShowDialog();
            JsonLoadTxt.Text = f.SelectedPath;
        }

        private void BrowseButton2(object sender, RoutedEventArgs e)
        {
            var f = new VistaFolderBrowserDialog();
            f.ShowDialog();
            JsonSaveTxt.Text = f.SelectedPath;
        }

        private void SaveClose(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.JsonSavePath = JsonSaveTxt.Text;
            Properties.Settings.Default.JsonLoadPath = JsonLoadTxt.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
        private void CancelClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
