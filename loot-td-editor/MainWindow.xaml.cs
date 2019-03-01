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
            Debug.WriteLine("Setting innatesLists");

            ArmorEditor.InnateBox.ItemsSource = InnateEditor.Affixes;
            WeaponEditor.InnateBox.ItemsSource = InnateEditor.Affixes;
            AccessoryEditor.InnateBox.ItemsSource = InnateEditor.Affixes;
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

                switch (t.Header.ToString())
                {
                    case "_Prefix":
                        SaveAffixJson(AffixType.PREFIX, PrefixEditor.Affixes);
                        break;
                    case "_Suffix":
                        SaveAffixJson(AffixType.SUFFIX, SuffixEditor.Affixes);
                        break;
                    case "_Enchantment":
                        SaveAffixJson(AffixType.ENCHANTMENT, EnchantmentEditor.Affixes);
                        break;
                    case "_Innate":
                        SaveAffixJson(AffixType.INNATE, InnateEditor.Affixes);
                        break;
                    default:
                        return;
                }
            } else if (t.Header is "_Equipment")
            {
                t = EquipTabControl.SelectedItem as TabItem;

                switch (t.Header.ToString())
                {
                    case "_Armor":
                        SaveToJson<EquipmentBase>("\\items\\armor", ArmorEditor.Equipments);
                        break;
                    case "_Weapon":
                        SaveToJson<EquipmentBase>("\\items\\weapon", WeaponEditor.Equipments);
                        break;
                    case "_Accessory":
                        SaveToJson<EquipmentBase>("\\items\\accessory", AccessoryEditor.Equipments);
                        break;
                    default:
                        return;
                }
            } else if (t.Header is "_Archetypes")
            {
                SaveToJson<ArchetypeBase>("\\archetypes\\archetypes", ArchetypeEditor.Archetypes);
            }
        }

        private void SaveAffixJson(AffixType type, List<AffixBase> affixList)
        {
            string fileName = type.ToString().ToLower();
            string filePath = "\\affixes\\" + fileName;
            SaveToJson<AffixBase>(filePath, affixList);
        }

        private void SaveToJson<T>(string path , List<T> list)
        {
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath is "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }
            string s = Properties.Settings.Default.JsonSavePath + path + ".json";
            string o = JsonConvert.SerializeObject(list);
            System.IO.File.WriteAllText(s, o);
        }

        private void SaveJsonAll(object sender, RoutedEventArgs e)
        {
            SaveToJson<EquipmentBase>("\\items\\armor", ArmorEditor.Equipments);
            SaveToJson<EquipmentBase>("\\items\\weapon", WeaponEditor.Equipments);
            SaveToJson<EquipmentBase>("\\items\\accessory", AccessoryEditor.Equipments);
            SaveAffixJson(AffixType.PREFIX, PrefixEditor.Affixes);
            SaveAffixJson(AffixType.SUFFIX, SuffixEditor.Affixes);
            SaveAffixJson(AffixType.ENCHANTMENT, EnchantmentEditor.Affixes);
            SaveAffixJson(AffixType.INNATE, InnateEditor.Affixes);
            //SaveToJson<AbilityBase>("\\abilities\\abilities", AbilityEditor.Abilities);
            SaveToJson<ArchetypeBase>("\\archetypes\\archetypes", ArchetypeEditor.Archetypes);
            MessageBox.Show("Save Complete", "Save Complete", MessageBoxButton.OK);
        }
    }
}
