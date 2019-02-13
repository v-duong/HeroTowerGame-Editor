using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;
using System.Windows.Forms;


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
