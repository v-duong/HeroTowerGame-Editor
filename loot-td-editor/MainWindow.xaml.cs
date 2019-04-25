using loot_td;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> locales = new List<string>() { "en-US" };
        public bool saveEquips = true;
        public bool saveAbility = true;
        public bool saveArchetype = true;
        public bool saveAffix = true;
        Helpers.ErrorLog ErrorLog = new Helpers.ErrorLog();

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
            if (t.Header.ToString() == "_Affixes")
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
            }
            else if (t.Header.ToString() == "_Equipment")
            {
                t = EquipTabControl.SelectedItem as TabItem;

                switch (t.Header.ToString())
                {
                    case "_Armor":
                        SaveToJson<EquipmentBase>("\\items\\armor", ArmorEditor.Equipments.ToList());
                        break;

                    case "_Weapon":
                        SaveToJson<EquipmentBase>("\\items\\weapon", WeaponEditor.Equipments.ToList());
                        break;

                    case "_Accessory":
                        SaveToJson<EquipmentBase>("\\items\\accessory", AccessoryEditor.Equipments.ToList());
                        break;

                    default:
                        return;
                }
            }
            else if (t.Header.ToString() == "_Archetypes")
            {
                SaveArchetypesJson();
            }
            else if (t.Header.ToString() == "_Abilities")
            {
                SaveAbilitiesJson();
            }
            SaveLocalizationKeys();
        }

        private void SaveAffixJson(AffixType type, List<AffixBase> affixList)
        {

            string fileName = type.ToString().ToLower();
            string filePath = "\\affixes\\" + fileName;
            SaveToJson<AffixBase>(filePath, affixList);
        }

        private void SaveToJson<T>(string path, List<T> list)
        {
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath == "")
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
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath == "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = AbilitySerializeResolver.Instance
            };

            if (!Helpers.ErrorCheckAbilities(AbilityEditor.Abilities.ToList(), ErrorLog))
            {
                saveAbility = false;
                return;
            }

            string s = Properties.Settings.Default.JsonSavePath + "\\abilities\\abilities.json";
            string o = JsonConvert.SerializeObject(AbilityEditor.Abilities.ToList(), settings);
            System.IO.File.WriteAllText(s, o);

            s = Properties.Settings.Default.JsonSavePath + "\\abilities\\abilities.editor.json";
            o = JsonConvert.SerializeObject(AbilityEditor.Abilities.ToList());
            System.IO.File.WriteAllText(s, o);
        }

        private void SaveArchetypesJson()
        {
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath == "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = ArchetypeSerializeResolver.Instance
            };

            if (!Helpers.ErrorCheckArchetypes(ArchetypeEditor.Archetypes.ToList(), ErrorLog))
            {
                saveArchetype = false;
                return;
            }

            string s = Properties.Settings.Default.JsonSavePath + "\\archetypes\\archetypes.json";
            string o = JsonConvert.SerializeObject(ArchetypeEditor.Archetypes.ToList(), settings);
            System.IO.File.WriteAllText(s, o);

            s = Properties.Settings.Default.JsonSavePath + "\\archetypes\\archetypes.editor.json";
            o = JsonConvert.SerializeObject(ArchetypeEditor.Archetypes.ToList());
            System.IO.File.WriteAllText(s, o);

            saveArchetype = true;
        }

        private void SaveJsonAll(object sender, RoutedEventArgs e)
        {
            ErrorLog.Clear();

            List<EquipmentBase> equipList = new List<EquipmentBase>();
            equipList.AddRange(ArmorEditor.Equipments.ToList());
            equipList.AddRange(WeaponEditor.Equipments.ToList());
            equipList.AddRange(AccessoryEditor.Equipments.ToList());

            if (!Helpers.ErrorCheckEquipment(equipList, ErrorLog))
            {
                saveEquips = false;
            } else
            {
                SaveToJson<EquipmentBase>("\\items\\armor", ArmorEditor.Equipments.ToList());
                SaveToJson<EquipmentBase>("\\items\\weapon", WeaponEditor.Equipments.ToList());
                SaveToJson<EquipmentBase>("\\items\\accessory", AccessoryEditor.Equipments.ToList());
                saveEquips = true;
            }

            List<AffixBase> a = new List<AffixBase>();
            a.AddRange(PrefixEditor.Affixes.ToList());
            a.AddRange(SuffixEditor.Affixes.ToList());
            a.AddRange(EnchantmentEditor.Affixes.ToList());
            a.AddRange(InnateEditor.Affixes.ToList());
            if (!Helpers.ErrorCheckAffixes(a, ErrorLog))
            {
                saveAffix = false;
            } else
            {
                SaveAffixJson(AffixType.PREFIX, PrefixEditor.Affixes.ToList());
                SaveAffixJson(AffixType.SUFFIX, SuffixEditor.Affixes.ToList());
                SaveAffixJson(AffixType.ENCHANTMENT, EnchantmentEditor.Affixes.ToList());
                SaveAffixJson(AffixType.INNATE, InnateEditor.Affixes.ToList());
                saveAffix = true;
            }
            
            SaveAbilitiesJson();
            //SaveToJson<ArchetypeBase>("\\archetypes\\archetypes", ArchetypeEditor.Archetypes.ToList());
            SaveArchetypesJson();
            SaveLocalizationKeys();
            if (ErrorLog.Count == 0)
            {
                MessageBox.Show("Save Complete", "Save Complete", MessageBoxButton.OK);
            } else
            {
                string s = "";
                foreach(KeyValuePair<string, int> x in ErrorLog.dict)
                {
                    s += x.Value + " " + x.Key + '\n';
                }
                MessageBox.Show(s, "Error", MessageBoxButton.OK);
            }
        }

        private void SaveLocalizationCommon(string locale)
        {
            string filepath = Properties.Settings.Default.JsonSavePath + "\\localization\\common." + locale + ".json";
            InitializeLocalizationSaving(locale, filepath, out SortedDictionary<string, string> localization, out HashSet<string> keys);

            List<string> bonusTypes = new List<string>(Enum.GetNames(typeof(BonusType)));

            foreach (string x in bonusTypes)
            {
                string localizationKey = "bonusType." + x;
                if (!localization.ContainsKey(localizationKey))
                    localization.Add(localizationKey, "");
                else
                    keys.Remove(localizationKey);
            }

            List<string> groupTypes = new List<string>(Enum.GetNames(typeof(GroupType)));

            foreach (string x in groupTypes)
            {
                string localizationKey = "groupType." + x;
                if (!localization.ContainsKey(localizationKey))
                    localization.Add(localizationKey, "");
                else
                    keys.Remove(localizationKey);
            }

            foreach (string key in keys)
            {
                localization.Remove(key);
            }

            string o = JsonConvert.SerializeObject(localization);
            System.IO.File.WriteAllText(filepath, o);
        }

        private void SaveLocalizationEquipment(string locale)
        {
            if (!saveEquips)
                return;
            string filepath = Properties.Settings.Default.JsonSavePath + "\\localization\\equipment." + locale + ".json";
            InitializeLocalizationSaving(locale, filepath, out SortedDictionary<string, string> localization, out HashSet<string> keys);

            List<EquipmentBase> equips = ArmorEditor.Equipments.ToList();
            equips.AddRange(WeaponEditor.Equipments.ToList());
            equips.AddRange(AccessoryEditor.Equipments.ToList());

            foreach (EquipmentBase equipment in equips)
            {
                string localizationKey = "equipment." + equipment.IdName;
                if (!localization.ContainsKey(localizationKey))
                    localization.Add(localizationKey, "");
                else
                    keys.Remove(localizationKey);
            }

            foreach (string key in keys)
            {
                localization.Remove(key);
            }

            string o = JsonConvert.SerializeObject(localization);
            System.IO.File.WriteAllText(filepath, o);
        }

        private void SaveLocalizationArchetype(string locale)
        {
            if (!saveArchetype)
                return;
            string filepath = Properties.Settings.Default.JsonSavePath + "\\localization\\archetype." + locale + ".json";
            InitializeLocalizationSaving(locale, filepath, out SortedDictionary<string, string> localization, out HashSet<string> keys);

            List<ArchetypeBase> archetypes = ArchetypeEditor.Archetypes.ToList();

            foreach (ArchetypeBase a in archetypes)
            {
                string localizationKey = "archetype." + a.IdName + ".name";
                if (!localization.ContainsKey(localizationKey))
                    localization.Add(localizationKey, "");
                else
                    keys.Remove(localizationKey);
                localizationKey = "archetype." + a.IdName + ".text";
                if (!localization.ContainsKey(localizationKey))
                    localization.Add(localizationKey, "");
                else
                    keys.Remove(localizationKey);
                foreach (ArchetypeSkillNode node in a.NodeList)
                {
                    if (node.Type == NodeType.GREATER || node.Type == NodeType.MASTER)
                    {
                        localizationKey = "archetype." + a.IdName + ".node." + node.IdName;
                        if (!localization.ContainsKey(localizationKey))
                            localization.Add(localizationKey, "");
                        else
                            keys.Remove(localizationKey);
                    }
                }
            }

            foreach (string key in keys)
            {
                localization.Remove(key);
            }

            string o = JsonConvert.SerializeObject(localization);
            System.IO.File.WriteAllText(filepath, o);
        }

        private void SaveLocalizationAbility(string locale)
        {
            if (!saveAbility)
                return;
            string filepath = Properties.Settings.Default.JsonSavePath + "\\localization\\ability." + locale + ".json";
            InitializeLocalizationSaving(locale, filepath, out SortedDictionary<string, string> localization, out HashSet<string> keys);

            List<AbilityBase> abilities = AbilityEditor.Abilities.ToList();

            foreach (AbilityBase a in abilities)
            {
                string localizationKey = "ability." + a.IdName + ".name";
                if (!localization.ContainsKey(localizationKey))
                    localization.Add(localizationKey, "");
                else
                    keys.Remove(localizationKey);
                localizationKey = "ability." + a.IdName + ".text";
                if (!localization.ContainsKey(localizationKey))
                    localization.Add(localizationKey, "");
                else
                    keys.Remove(localizationKey);
            }

            foreach (string key in keys)
            {
                localization.Remove(key);
            }

            string o = JsonConvert.SerializeObject(localization);
            System.IO.File.WriteAllText(filepath, o);
        }

        private void InitializeLocalizationSaving(string locale, string filepath, out SortedDictionary<string, string> localization, out HashSet<string> keys)
        {
            if (!System.IO.File.Exists(filepath))
            {
                localization = new SortedDictionary<string, string>();
            }
            else
            {
                string json = System.IO.File.ReadAllText(filepath);
                localization = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(json);
            }

            keys = new HashSet<string>(localization.Keys);
        }

        private void SaveLocalizationKeys()
        {
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath == "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }
            foreach (string locale in locales)
            {
                //SaveLocalizationCommon(locale);
                SaveLocalizationEquipment(locale);
                SaveLocalizationAbility(locale);
                SaveLocalizationArchetype(locale);
                /*
                List<AffixBase> affixes = PrefixEditor.Affixes.ToList();
                affixes.AddRange(SuffixEditor.Affixes.ToList());
                affixes.AddRange(EnchantmentEditor.Affixes.ToList());
                affixes.AddRange(InnateEditor.Affixes.ToList());

                foreach (AffixBase a in affixes)
                {
                    if (!localization.ContainsKey(a.IdName))
                        localization.Add("affix." + a.IdName, "");
                    else
                        keys.Remove("affix." + a.IdName);
                }
                */
            }
        }
    }
}

public class AbilitySerializeResolver : DefaultContractResolver
{
    public static readonly AbilitySerializeResolver Instance = new AbilitySerializeResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyName == "BaseAbilityPower" || property.PropertyName == "AbilityScaling" || property.PropertyName == "MinMult" || property.PropertyName == "MaxMult")
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

public class ArchetypeSerializeResolver : DefaultContractResolver
{
    public static readonly ArchetypeSerializeResolver Instance = new ArchetypeSerializeResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyName == "ChildrenEditor")
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