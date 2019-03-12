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
using Newtonsoft.Json.Serialization;
using System.Reflection;

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

            ArmorEditor.innatesList = InnateEditor.Affixes;
            WeaponEditor.innatesList = InnateEditor.Affixes;
            AccessoryEditor.innatesList = InnateEditor.Affixes;

            ArmorEditor.InnateBox.ItemsSource = InnateEditor.Affixes;
            WeaponEditor.InnateBox.ItemsSource = InnateEditor.Affixes;
            AccessoryEditor.InnateBox.ItemsSource = InnateEditor.Affixes;

            ArchetypeEditor.NodeAbilityList.ItemsSource = AbilityEditor.Abilities;

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
                        SaveAffixJson(AffixType.PREFIX, PrefixEditor.Affixes.ToList());
                        break;
                    case "_Suffix":
                        SaveAffixJson(AffixType.SUFFIX, SuffixEditor.Affixes.ToList());
                        break;
                    case "_Enchantment":
                        SaveAffixJson(AffixType.ENCHANTMENT, EnchantmentEditor.Affixes.ToList());
                        break;
                    case "_Innate":
                        SaveAffixJson(AffixType.INNATE, InnateEditor.Affixes.ToList());
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
                SaveToJson<ArchetypeBase>("\\archetypes\\archetypes", ArchetypeEditor.Archetypes.ToList());
            } else if (t.Header is "_Abilities")
            {
                SaveAbilitiesJson();
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

        private void SaveAbilitiesJson()
        {
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath is "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = ShouldSerializeContractResolver.Instance;

            string s = Properties.Settings.Default.JsonSavePath + "\\abilities\\abilities.json";
            string o = JsonConvert.SerializeObject(AbilityEditor.Abilities.ToList(), settings);
            System.IO.File.WriteAllText(s, o);



            s = Properties.Settings.Default.JsonSavePath + "\\abilities\\abilities.editor.json";
            o = JsonConvert.SerializeObject(AbilityEditor.Abilities.ToList());
            System.IO.File.WriteAllText(s, o);
        }

        private void SaveJsonAll(object sender, RoutedEventArgs e)
        {
            SaveToJson<EquipmentBase>("\\items\\armor", ArmorEditor.Equipments);
            SaveToJson<EquipmentBase>("\\items\\weapon", WeaponEditor.Equipments);
            SaveToJson<EquipmentBase>("\\items\\accessory", AccessoryEditor.Equipments);
            SaveAffixJson(AffixType.PREFIX, PrefixEditor.Affixes.ToList());
            SaveAffixJson(AffixType.SUFFIX, SuffixEditor.Affixes.ToList());
            SaveAffixJson(AffixType.ENCHANTMENT, EnchantmentEditor.Affixes.ToList());
            SaveAffixJson(AffixType.INNATE, InnateEditor.Affixes.ToList());
            SaveAbilitiesJson();
            SaveToJson<ArchetypeBase>("\\archetypes\\archetypes", ArchetypeEditor.Archetypes.ToList());
            MessageBox.Show("Save Complete", "Save Complete", MessageBoxButton.OK);
        }


    }
}

public class ShouldSerializeContractResolver : DefaultContractResolver
{
    public new static readonly ShouldSerializeContractResolver Instance = new ShouldSerializeContractResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyName == "BaseAbilityPower" || property.PropertyName == "AbilityScaling")
        {
            property.ShouldSerialize =
                instance =>
                {
                    return false;
                };
        }

        return property;
    }
}