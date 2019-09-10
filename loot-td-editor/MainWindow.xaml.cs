using loot_td;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        public bool saveEnemy = true;
        public bool saveStage = true;

        private int equipmentEditorCount = 0;
        private int archetypeEditorCount = 0;
        private int enemyEditorCount = 0;
        private int stageEditorCount = 0;
        private int affixEditorCount = 0;
        private int uniqueEditorCount = 0;

        private Helpers.ErrorLog ErrorLog = new Helpers.ErrorLog();

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Setting innatesLists");

            ArmorEditor.innatesList = InnateEditor.Affixes;
            WeaponEditor.innatesList = InnateEditor.Affixes;
            AccessoryEditor.innatesList = InnateEditor.Affixes;

            ArmorEditor.BaseFields.InnateBox.ItemsSource = InnateEditor.Affixes;
            WeaponEditor.BaseFields.InnateBox.ItemsSource = InnateEditor.Affixes;
            AccessoryEditor.BaseFields.InnateBox.ItemsSource = InnateEditor.Affixes;

            ArchetypeEditor.NodeAbilityList.ItemsSource = AbilityEditor.Abilities;
            EnemyEditor.DataGridAbilityName.ItemsSource = AbilityEditor.Abilities;

            StageEditor.EnemyComboBox.ItemsSource = EnemyEditor.EnemyBaseList;

            CompositeCollection cc = new CompositeCollection
            {
                new CollectionContainer() { Collection =  ArmorEditor.Equipments},
                new CollectionContainer() { Collection =  WeaponEditor.Equipments},
                new CollectionContainer() { Collection =  AccessoryEditor.Equipments}
            };

            CollectionViewSource viewSource = new CollectionViewSource
            {
                Source = cc
            };

            StageEditor.EquipmentComboBox.ItemsSource = cc;
            StageEditor.ArchetypeComboBox.ItemsSource = ArchetypeEditor.Archetypes;
            StageEditor.StagePropertiesComboBox.ItemsSource = EnemyAffixEditor.Affixes;
            StageEditor.WaveModComboBox.ItemsSource = EnemyAffixEditor.Affixes;

            GetCounts();
        }

        private void GetCounts()
        {
            equipmentEditorCount = ArmorEditor.Equipments.Count;
            equipmentEditorCount += WeaponEditor.Equipments.Count;
            equipmentEditorCount += AccessoryEditor.Equipments.Count;

            uniqueEditorCount = UniqueItemEditor.Uniques.Count;

            affixEditorCount = PrefixEditor.Affixes.Count + SuffixEditor.Affixes.Count + EnchantmentEditor.Affixes.Count + InnateEditor.Affixes.Count + EnemyAffixEditor.Affixes.Count;

            stageEditorCount = StageEditor.Stages.Count;
            enemyEditorCount = EnemyEditor.EnemyBaseList.Count;

            archetypeEditorCount = ArchetypeEditor.Archetypes.Count;
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

                    case "_Enemy":
                        SaveAffixJson(AffixType.MONSTERMOD, EnemyAffixEditor.Affixes.ToList());
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

            foreach (ArchetypeBase archetype in ArchetypeEditor.Archetypes.ToList())
            {
                archetype.NodeList = new ObservableCollection<ArchetypeSkillNode>(archetype.NodeList.OrderBy(x => x.Id));
            }

            string s = Properties.Settings.Default.JsonSavePath + "\\archetypes\\archetypes.json";
            string o = JsonConvert.SerializeObject(ArchetypeEditor.Archetypes.ToList(), settings);
            System.IO.File.WriteAllText(s, o);

            s = Properties.Settings.Default.JsonSavePath + "\\archetypes\\archetypes.editor.json";
            o = JsonConvert.SerializeObject(ArchetypeEditor.Archetypes.ToList());
            System.IO.File.WriteAllText(s, o);

            saveArchetype = true;
        }

        private void SaveEnemiesJson()
        {
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath == "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = EnemySerializeResolver.Instance
            };

            if (!Helpers.ErrorCheckEnemies(EnemyEditor.EnemyBaseList.ToList(), ErrorLog))
            {
                saveEnemy = false;
                return;
            }

            string s = Properties.Settings.Default.JsonSavePath + "\\enemies\\enemies.json";
            string o = JsonConvert.SerializeObject(EnemyEditor.EnemyBaseList.ToList(), settings);
            System.IO.File.WriteAllText(s, o);

            s = Properties.Settings.Default.JsonSavePath + "\\enemies\\enemies.editor.json";
            o = JsonConvert.SerializeObject(EnemyEditor.EnemyBaseList.ToList());
            System.IO.File.WriteAllText(s, o);

            saveEnemy = true;
        }

        private void SaveStagesJson()
        {
            if (Properties.Settings.Default.JsonSavePath == null || Properties.Settings.Default.JsonSavePath == "")
            {
                MessageBox.Show("Save Path not defined", "Error", MessageBoxButton.OK);
                return;
            }

            /*
            if (!Helpers.ErrorCheckEnemies(EnemyEditor.EnemyBaseList.ToList(), ErrorLog))
            {
                saveEnemy = false;
                return;
            }
            */
            string s = Properties.Settings.Default.JsonSavePath + "\\stages\\stages.json";
            string o = JsonConvert.SerializeObject(StageEditor.Stages.ToList());
            System.IO.File.WriteAllText(s, o);

            saveStage = true;
        }

        private void SaveJsonAll(object sender, RoutedEventArgs e)
        {
            ErrorLog.Clear();

            List<EquipmentBase> equipList = new List<EquipmentBase>();
            equipList.AddRange(ArmorEditor.Equipments.ToList());
            equipList.AddRange(WeaponEditor.Equipments.ToList());
            equipList.AddRange(AccessoryEditor.Equipments.ToList());

            List<AffixBase> a = new List<AffixBase>();
            a.AddRange(PrefixEditor.Affixes.ToList());
            a.AddRange(SuffixEditor.Affixes.ToList());
            a.AddRange(EnchantmentEditor.Affixes.ToList());
            a.AddRange(InnateEditor.Affixes.ToList());
            a.AddRange(EnemyAffixEditor.Affixes.ToList());

            int equipCount = equipList.Count;
            int affixCount = a.Count;
            int stageCount = StageEditor.Stages.Count;
            int enemyCount = EnemyEditor.EnemyBaseList.Count;
            int archetypeCount = ArchetypeEditor.Archetypes.Count;
            int uniqueCount = UniqueItemEditor.Uniques.Count;

            string confirmString = "";
            bool noChanges = false;

            if (equipCount > equipmentEditorCount)
            {
                confirmString += "Equipment Entry +" + Math.Abs(equipCount - equipmentEditorCount) + "\n";
            }
            else if (equipCount < equipmentEditorCount)
            {
                confirmString += "Equipment Entry -" + Math.Abs(equipCount - equipmentEditorCount) + "\n";
            }

            if (affixCount > affixEditorCount)
            {
                confirmString += "Affix Entry +" + Math.Abs(affixCount - affixEditorCount) + "\n";
            }
            else if (affixCount < affixEditorCount)
            {
                confirmString += "Affix Entry -" + Math.Abs(affixCount - affixEditorCount) + "\n";
            }

            if (archetypeCount > archetypeEditorCount)
            {
                confirmString += "Archetype Entry +" + Math.Abs(archetypeCount - archetypeEditorCount) + "\n";
            }
            else if (archetypeCount < archetypeEditorCount)
            {
                confirmString += "Archetype Entry -" + Math.Abs(archetypeCount - archetypeEditorCount) + "\n";
            }

            if (enemyCount > enemyEditorCount)
            {
                confirmString += "Enemy Entry +" + Math.Abs(enemyCount - enemyEditorCount) + "\n";
            }
            else if (enemyCount < enemyEditorCount)
            {
                confirmString += "Enemy Entry -" + Math.Abs(enemyCount - enemyEditorCount) + "\n";
            }

            if (stageCount > stageEditorCount)
            {
                confirmString += "Stage Entry +" + Math.Abs(stageCount - stageEditorCount) + "\n";
            }
            else if (stageCount < stageEditorCount)
            {
                confirmString += "Stage Entry -" + Math.Abs(stageCount - stageEditorCount) + "\n";
            }

            if (uniqueCount > uniqueEditorCount)
            {
                confirmString += "Unique Item Entry +" + Math.Abs(uniqueCount - uniqueEditorCount) + "\n";
            }
            else if (uniqueCount < uniqueEditorCount)
            {
                confirmString += "Unique Item Entry -" + Math.Abs(uniqueCount - uniqueEditorCount) + "\n";
            }

            if (confirmString == "")
            {
                confirmString = "No Changes in Entry Counts";
                noChanges = true;
            }

            confirmString += "\nProceed with Saving?";
            MessageBoxResult result = MessageBox.Show(confirmString, "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
                return;

            if (!Helpers.ErrorCheckEquipment(equipList, ErrorLog))
            {
                saveEquips = false;
            }
            else
            {
                SaveToJson("\\items\\armor", ArmorEditor.Equipments.ToList());
                SaveToJson("\\items\\weapon", WeaponEditor.Equipments.ToList());
                SaveToJson("\\items\\accessory", AccessoryEditor.Equipments.ToList());
                SaveToJson("\\items\\unique", UniqueItemEditor.Uniques.ToList());
                saveEquips = true;
            }

            if (!Helpers.ErrorCheckAffixes(a, ErrorLog))
            {
                saveAffix = false;
            }
            else
            {
                SaveAffixJson(AffixType.PREFIX, PrefixEditor.Affixes.ToList());
                SaveAffixJson(AffixType.SUFFIX, SuffixEditor.Affixes.ToList());
                SaveAffixJson(AffixType.ENCHANTMENT, EnchantmentEditor.Affixes.ToList());
                SaveAffixJson(AffixType.INNATE, InnateEditor.Affixes.ToList());
                SaveAffixJson(AffixType.MONSTERMOD, EnemyAffixEditor.Affixes.ToList());
                saveAffix = true;
            }

            SaveAbilitiesJson();
            SaveEnemiesJson();
            SaveStagesJson();
            //SaveToJson<ArchetypeBase>("\\archetypes\\archetypes", ArchetypeEditor.Archetypes.ToList());
            SaveArchetypesJson();
            SaveLocalizationKeys();
            if (ErrorLog.Count == 0)
            {
                MessageBox.Show("Save Complete", "Save Complete", MessageBoxButton.OK);
                GetCounts();
            }
            else
            {
                string s = "";
                foreach (KeyValuePair<string, int> x in ErrorLog.dict)
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

        private void SaveLocalizationEnemy(string locale)
        {
            if (!saveEnemy)
                return;
            string filepath = Properties.Settings.Default.JsonSavePath + "\\localization\\enemy." + locale + ".json";
            InitializeLocalizationSaving(locale, filepath, out SortedDictionary<string, string> localization, out HashSet<string> keys);

            List<EnemyBase> enemies = EnemyEditor.EnemyBaseList.ToList();

            foreach (EnemyBase a in enemies)
            {
                string localizationKey = "enemy." + a.IdName + ".name";
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

        private void SaveLocalizationStage(string locale)
        {
            if (!saveStage)
                return;
            string filepath = Properties.Settings.Default.JsonSavePath + "\\localization\\stage." + locale + ".json";
            InitializeLocalizationSaving(locale, filepath, out SortedDictionary<string, string> localization, out HashSet<string> keys);

            List<StageInfoCollection> stage = StageEditor.Stages.ToList();

            foreach (StageInfoCollection a in stage)
            {
                string localizationKey = "stage." + a.IdName + ".name";
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
                SaveLocalizationEnemy(locale);
                SaveLocalizationStage(locale);
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

        private void ReloadLocalization_Click(object sender, RoutedEventArgs e)
        {
            Localization.LoadStrings();
            if (UniqueItemEditor.EquipList.SelectedItem != null)
            {
                UniqueBase u = UniqueItemEditor.EquipList.SelectedItem as UniqueBase;
                u.RaisePropertyAffixString_(null, null);
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

public class EnemySerializeResolver : DefaultContractResolver
{
    public static readonly EnemySerializeResolver Instance = new EnemySerializeResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyName == "ActNumber")
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