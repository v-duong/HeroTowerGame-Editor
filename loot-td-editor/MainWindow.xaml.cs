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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using loot_td;
using Newtonsoft.Json;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void JsonSettingsClick(object sender, RoutedEventArgs e)
        {
            JSONSettingsWindow JsonSettingsWindow = new JSONSettingsWindow();
            JsonSettingsWindow.Show();
        }

        private void SaveOneJsonClick(object sender, RoutedEventArgs e)
        {
            TabItem t = Cat1TabControl.SelectedItem as TabItem;
            if (t.Header is "_Affixes")
            {
                t = AffixesTabControl.SelectedItem as TabItem;
            }
            switch (t.Header.ToString()) {
                case "_Prefix":
                    SaveAffixJson(AffixType.PREFIX, PrefixEditor.Affixes);
                    break;
                case "_Suffix":
                    SaveAffixJson(AffixType.SUFFIX, SuffixEditor.Affixes);
                    break;
                case "_Enchantment":
                    SaveAffixJson(AffixType.ENCHANTMENT, SuffixEditor.Affixes);
                    break;
                case "_Innate":
                    SaveAffixJson(AffixType.INNATE, SuffixEditor.Affixes);
                    break;
                default:
                    return;
            }
        }

        private void SaveAffixJson(AffixType type, List<AffixBase> affixList)
        {
            string fileName = type.ToString().ToLower() + ".json";
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath is "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }
            string filePath = Properties.Settings.Default.JsonSavePath + "\\affixes\\" + fileName;
            string o = JsonConvert.SerializeObject(affixList);
            System.IO.File.WriteAllText(filePath, o);
            Debug.WriteLine(o);
        }
    }
}
