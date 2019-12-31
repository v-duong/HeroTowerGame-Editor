using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace loot_td_editor
{
    public partial class UniqueEditor : UserControl
    {
        public ObservableCollection<AffixBase> innatesList { get; set; }
        public ObservableCollection<UniqueBase> Uniques;
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList(); } }
        public IList<EquipSlotType> EquipSlotTypes { get { return Enum.GetValues(typeof(EquipSlotType)).Cast<EquipSlotType>().ToList(); } }

        private int currentID = 0;

        public void InitializeEquipments()
        {
            string fileName = "unique.json";
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;

            string filePath = Properties.Settings.Default.JsonLoadPath + "\\items\\" + fileName;
            if (!System.IO.File.Exists(filePath))
            {
                Uniques = new ObservableCollection<UniqueBase>();
                EquipList.ItemsSource = Uniques;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Uniques = JsonConvert.DeserializeObject<ObservableCollection<UniqueBase>>(json);

            foreach (UniqueBase k in Uniques)
            {
                if (k.Description == null)
                    k.Description = "";
                foreach (AffixBase b in k.FixedUniqueAffixes)
                {
                    b.AffixBonuses.CollectionChanged += b.RaiseListStringChanged;
                    b.AffixBonuses.CollectionChanged += k.RaisePropertyAffixString;

                    foreach (AffixBonus bonus in b.AffixBonuses)
                    {
                        bonus.PropertyChanged += b.RaiseListStringChanged_;
                        bonus.PropertyChanged += k.RaisePropertyAffixString_;
                    }
                }
                foreach (AffixBase b in k.RandomUniqueAffixes)
                {
                    b.AffixBonuses.CollectionChanged += b.RaiseListStringChanged;
                    b.AffixBonuses.CollectionChanged += k.RaisePropertyAffixString;

                    foreach (AffixBonus bonus in b.AffixBonuses)
                    {
                        bonus.PropertyChanged += b.RaiseListStringChanged_;
                        bonus.PropertyChanged += k.RaisePropertyAffixString_;
                    }
                }
            }

            EquipList.ItemsSource = Uniques;
        }

        public UniqueEditor()
        {
            InitializeComponent();
            InitializeEquipments();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            UniqueBase temp = new UniqueBase
            {
                IdName = "UNTITLED NEW",
                InnateAffixId = null
            };
            Uniques.Add(temp);
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            UniqueBase temp = Helpers.DeepClone((UniqueBase)EquipList.SelectedItem);
            Uniques.Add(temp);
            foreach (AffixBase b in temp.FixedUniqueAffixes)
            {
                b.AffixBonuses.CollectionChanged += b.RaiseListStringChanged;
                b.AffixBonuses.CollectionChanged += temp.RaisePropertyAffixString;

                foreach (AffixBonus bonus in b.AffixBonuses)
                {
                    bonus.PropertyChanged += b.RaiseListStringChanged_;
                    bonus.PropertyChanged += temp.RaisePropertyAffixString_;
                }
            }
            foreach (AffixBase b in temp.RandomUniqueAffixes)
            {
                b.AffixBonuses.CollectionChanged += b.RaiseListStringChanged;
                b.AffixBonuses.CollectionChanged += temp.RaisePropertyAffixString;

                foreach (AffixBonus bonus in b.AffixBonuses)
                {
                    bonus.PropertyChanged += b.RaiseListStringChanged_;
                    bonus.PropertyChanged += temp.RaisePropertyAffixString_;
                }
            }
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;

            string name = ((UniqueBase)EquipList.SelectedItem).IdName;

            MessageBoxResult res = System.Windows.MessageBox.Show("Delete " + name + "?", "Confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.No)
                return;

            Uniques.Remove((UniqueBase)EquipList.SelectedItem);
        }

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            var dataView =
              (ListCollectionView)CollectionViewSource.GetDefaultView(EquipList.ItemsSource);

            dataView.SortDescriptions.Clear();

            if (sortBy == "IdName")
            {
                dataView.CustomSort = new NaturalStringComparer();
            }
            else
            {
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
            }
            dataView.Refresh();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            Uniques = new ObservableCollection<UniqueBase>(Uniques.OrderBy(x => x.IdName, new NaturalStringComparer2()));
            EquipList.ItemsSource = Uniques;
        }

        private void ReqButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (UniqueBase equipment in Uniques)
            {
                EquipBaseFields.CalculateReqValues(equipment);
            }
        }

        private void ValueButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (UniqueBase equipment in Uniques)
            {
                EquipBaseFields.CalculateArmorValues(equipment);
            }
        }

        private void FixedAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;
            AffixBase temp = new AffixBase
            {
                IdName = uniqueBase.IdName + "F" + uniqueBase.FixedUniqueAffixes.Count,
                AffixType = AffixType.UNIQUE
            };
            AddPropertyListeners(uniqueBase, temp);

            uniqueBase.FixedUniqueAffixes.Add(temp);
            SortFixedAffixes();
        }

        private void FixedCopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            if (FixedAffixesList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;

            AffixBase temp = Helpers.DeepClone((AffixBase)FixedAffixesList.SelectedItem);
            temp.IdName = uniqueBase.IdName + "F" + uniqueBase.FixedUniqueAffixes.Count;
            AddPropertyListeners(uniqueBase, temp);

            uniqueBase.FixedUniqueAffixes.Add(temp);
            SortFixedAffixes();
        }

        private void FixedRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            if (FixedAffixesList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;

            uniqueBase.FixedUniqueAffixes.Remove(FixedAffixesList.SelectedItem as AffixBase);
            SortFixedAffixes();
        }

        private void SortFixedAffixes()
        {
            if (EquipList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;

            for (int i = 0; i < uniqueBase.FixedUniqueAffixes.Count; i++)
            {
                uniqueBase.FixedUniqueAffixes[i].IdName = uniqueBase.IdName + "F" + i;
            }

            uniqueBase.RaisePropertyAffixString_(null, null);
        }

        private void RandomAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;
            AffixBase temp = new AffixBase
            {
                IdName = uniqueBase.IdName + "R" + uniqueBase.RandomUniqueAffixes.Count,
                AffixType = AffixType.UNIQUE
            };
            AddPropertyListeners(uniqueBase, temp);

            uniqueBase.RandomUniqueAffixes.Add(temp);
            SortRandomAffixes();
        }

        private void RandomCopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            if (RandomAffixesList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;

            AffixBase temp = Helpers.DeepClone((AffixBase)RandomAffixesList.SelectedItem);
            temp.IdName = uniqueBase.IdName + "R" + uniqueBase.RandomUniqueAffixes.Count;
            AddPropertyListeners(uniqueBase, temp);
            uniqueBase.RandomUniqueAffixes.Add(temp);
            SortRandomAffixes();
        }

        private static void AddPropertyListeners(UniqueBase uniqueBase, AffixBase temp)
        {
            temp.AffixBonuses.CollectionChanged += temp.RaiseListStringChanged;
            temp.AffixBonuses.CollectionChanged += uniqueBase.RaisePropertyAffixString;
        }

        private void RandomRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            if (RandomAffixesList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;

            uniqueBase.RandomUniqueAffixes.Remove(RandomAffixesList.SelectedItem as AffixBase);
            SortRandomAffixes();
        }

        private void SortRandomAffixes()
        {
            if (EquipList.SelectedItem == null)
                return;
            UniqueBase uniqueBase = EquipList.SelectedItem as UniqueBase;

            for (int i = 0; i < uniqueBase.RandomUniqueAffixes.Count; i++)
            {
                uniqueBase.RandomUniqueAffixes[i].IdName = uniqueBase.IdName + "R" + i;
            }
            uniqueBase.RaisePropertyAffixString_(null, null);
        }

        private void RandomAffixesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            ListView listView = sender as ListView;
            if (listView.SelectedItem == null)
                return;

            BonusPropGrid.SelectedAffix = listView.SelectedItem as AffixBase;

            UniqueBase unique = EquipList.SelectedItem as UniqueBase;
            Dictionary<BonusType, StatBonus> maxStatBonuses = new Dictionary<BonusType, StatBonus>();
            Dictionary<BonusType, StatBonus> minStatBonuses = new Dictionary<BonusType, StatBonus>();

            foreach (AffixBase x in unique.FixedUniqueAffixes)
            {
                foreach (AffixBonus y in x.AffixBonuses)
                {
                    if (!maxStatBonuses.ContainsKey(y.BonusType))
                    {
                        maxStatBonuses.Add(y.BonusType, new StatBonus());
                        minStatBonuses.Add(y.BonusType, new StatBonus());
                    }
                    maxStatBonuses[y.BonusType].AddBonus(y.ModifyType, y.MaxValue);
                    minStatBonuses[y.BonusType].AddBonus(y.ModifyType, y.MinValue);
                }
            }


            CalculateWeaponDps(unique, maxStatBonuses);
        }

        private void CalculateWeaponDps(UniqueBase unique, Dictionary<BonusType, StatBonus> maxStatBonuses)
        {
            Dictionary<ElementType, DamageStore> weaponDamage = new Dictionary<ElementType, DamageStore>();
            foreach (ElementType element in Enum.GetValues(typeof(ElementType)))
            {
                weaponDamage[element] = new DamageStore(0,0);
            }
            weaponDamage[0].Min = CalculateStat(unique.MinDamage, maxStatBonuses, BonusType.LOCAL_PHYSICAL_DAMAGE_MIN);
            weaponDamage[0].Max = CalculateStat(unique.MaxDamage, maxStatBonuses, BonusType.LOCAL_PHYSICAL_DAMAGE_MAX);
            weaponDamage[0].Min = CalculateStat(weaponDamage[0].Min, maxStatBonuses, BonusType.LOCAL_PHYSICAL_DAMAGE);
            weaponDamage[0].Max = CalculateStat(weaponDamage[0].Max, maxStatBonuses, BonusType.LOCAL_PHYSICAL_DAMAGE);

            weaponDamage[ElementType.FIRE].Min = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_FIRE_DAMAGE_MIN);
            weaponDamage[ElementType.FIRE].Max = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_FIRE_DAMAGE_MAX);
            weaponDamage[ElementType.FIRE].Min = CalculateStat(weaponDamage[ElementType.FIRE].Min, maxStatBonuses, BonusType.LOCAL_FIRE_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);
            weaponDamage[ElementType.FIRE].Max = CalculateStat(weaponDamage[ElementType.FIRE].Max, maxStatBonuses, BonusType.LOCAL_FIRE_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);

            weaponDamage[ElementType.COLD].Min = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_COLD_DAMAGE_MIN);
            weaponDamage[ElementType.COLD].Max = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_COLD_DAMAGE_MAX);
            weaponDamage[ElementType.COLD].Min = CalculateStat(weaponDamage[ElementType.COLD].Min, maxStatBonuses, BonusType.LOCAL_COLD_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);
            weaponDamage[ElementType.COLD].Max = CalculateStat(weaponDamage[ElementType.COLD].Max, maxStatBonuses, BonusType.LOCAL_COLD_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);

            weaponDamage[ElementType.LIGHTNING].Min = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_LIGHTNING_DAMAGE_MIN);
            weaponDamage[ElementType.LIGHTNING].Max = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_LIGHTNING_DAMAGE_MAX);
            weaponDamage[ElementType.LIGHTNING].Min = CalculateStat(weaponDamage[ElementType.LIGHTNING].Min, maxStatBonuses, BonusType.LOCAL_LIGHTNING_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);
            weaponDamage[ElementType.LIGHTNING].Max = CalculateStat(weaponDamage[ElementType.LIGHTNING].Max, maxStatBonuses, BonusType.LOCAL_LIGHTNING_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);

            weaponDamage[ElementType.EARTH].Min = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_EARTH_DAMAGE_MIN);
            weaponDamage[ElementType.EARTH].Max = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_EARTH_DAMAGE_MAX);
            weaponDamage[ElementType.EARTH].Min = CalculateStat(weaponDamage[ElementType.EARTH].Min, maxStatBonuses, BonusType.LOCAL_EARTH_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);
            weaponDamage[ElementType.EARTH].Max = CalculateStat(weaponDamage[ElementType.EARTH].Max, maxStatBonuses, BonusType.LOCAL_EARTH_DAMAGE, BonusType.LOCAL_ELEMENTAL_DAMAGE);

            weaponDamage[ElementType.DIVINE].Min = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_DIVINE_DAMAGE_MIN);
            weaponDamage[ElementType.DIVINE].Max = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_DIVINE_DAMAGE_MAX);
            weaponDamage[ElementType.DIVINE].Min = CalculateStat(weaponDamage[ElementType.DIVINE].Min, maxStatBonuses, BonusType.LOCAL_DIVINE_DAMAGE, BonusType.LOCAL_PRIMORDIAL_DAMAGE);
            weaponDamage[ElementType.DIVINE].Max = CalculateStat(weaponDamage[ElementType.DIVINE].Max, maxStatBonuses, BonusType.LOCAL_DIVINE_DAMAGE, BonusType.LOCAL_PRIMORDIAL_DAMAGE);

            weaponDamage[ElementType.VOID].Min = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_VOID_DAMAGE_MIN);
            weaponDamage[ElementType.VOID].Max = CalculateStat(0, maxStatBonuses, BonusType.LOCAL_VOID_DAMAGE_MAX);
            weaponDamage[ElementType.VOID].Min = CalculateStat(weaponDamage[ElementType.VOID].Min, maxStatBonuses, BonusType.LOCAL_VOID_DAMAGE, BonusType.LOCAL_PRIMORDIAL_DAMAGE);
            weaponDamage[ElementType.VOID].Max = CalculateStat(weaponDamage[ElementType.VOID].Max, maxStatBonuses, BonusType.LOCAL_VOID_DAMAGE, BonusType.LOCAL_PRIMORDIAL_DAMAGE);

            float AttackSpeed = CalculateStat(unique.AttackSpeed, maxStatBonuses, BonusType.LOCAL_ATTACK_SPEED);
            float pDps = (weaponDamage[0].Min + weaponDamage[0].Max) / 2f * AttackSpeed;

            int eMin = weaponDamage[ElementType.FIRE].Min + weaponDamage[ElementType.COLD].Min + weaponDamage[ElementType.LIGHTNING].Min + weaponDamage[ElementType.EARTH].Min;
            int eMax = weaponDamage[ElementType.FIRE].Max + weaponDamage[ElementType.COLD].Max + weaponDamage[ElementType.LIGHTNING].Max + weaponDamage[ElementType.EARTH].Max;
            float eDps = (eMin + eMax) / 2f * AttackSpeed;

            int aMin = weaponDamage[ElementType.DIVINE].Min + weaponDamage[ElementType.VOID].Min;
            int aMax = weaponDamage[ElementType.DIVINE].Max + weaponDamage[ElementType.VOID].Max;
            float aDps = (aMin + aMax) / 2f * AttackSpeed;

            pDpsLabel.Content = "pDps: " + pDps;
            eDpsLabel.Content = "eDps: " + eDps;
            aDpsLabel.Content = "aDps: " + aDps;

        }

        protected static int CalculateStat(int stat, Dictionary<BonusType, StatBonus> dic, params BonusType[] bonusTypes)
        {
            return (int)CalculateStat((float)stat, dic, bonusTypes);
        }

        protected static float CalculateStat(float stat, Dictionary<BonusType, StatBonus> dic, params BonusType[] bonusTypes)
        {
            StatBonus totalBonus = new StatBonus();
            foreach (BonusType bonusType in bonusTypes)
            {
                if (dic.TryGetValue(bonusType, out StatBonus bonus))
                {
                    totalBonus.AddBonuses(bonus);
                }
            }
            return totalBonus.CalculateStat(stat);
        }

        private void FixedAffixesList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            ListView listView = sender as ListView;
            if (listView.SelectedItem == null)
                return;

            BonusPropGrid.SelectedAffix = listView.SelectedItem as AffixBase;
        }
    }

    public class StatBonus
    {
        public float FlatModifier { get; private set; }
        public int AdditiveModifier { get; private set; }
        public List<float> MultiplyModifiers { get; private set; }
        public float CurrentMultiplier { get; private set; }
        public bool isStatOutdated;
        public bool HasFixedModifier { get; private set; }
        public float FixedModifier { get; private set; }

        public StatBonus()
        {
            FlatModifier = 0;
            AdditiveModifier = 0;
            MultiplyModifiers = new List<float>();
            CurrentMultiplier = 1.00f;
            HasFixedModifier = false;
            FixedModifier = 0;
            isStatOutdated = true;
        }

        public void ResetBonus()
        {
            FlatModifier = 0;
            AdditiveModifier = 0;
            MultiplyModifiers.Clear();
            CurrentMultiplier = 1.00f;
            HasFixedModifier = false;
            FixedModifier = 0;
            isStatOutdated = true;
        }

        public bool IsZero()
        {
            if (HasFixedModifier == true)
                return false;
            if (AdditiveModifier != 0 || MultiplyModifiers.Count != 0 || FlatModifier != 0)
                return false;
            return true;
        }

        public void AddBonuses(StatBonus otherBonus, bool overwriteFixed = true)
        {
            if (otherBonus == null)
                return;

            FlatModifier += otherBonus.FlatModifier;
            AdditiveModifier += otherBonus.AdditiveModifier;
            MultiplyModifiers.AddRange(otherBonus.MultiplyModifiers);
            if (otherBonus.HasFixedModifier && (HasFixedModifier && overwriteFixed || !HasFixedModifier))
            {
                HasFixedModifier = true;
                FixedModifier = otherBonus.FixedModifier;
            }
            UpdateCurrentMultiply();
        }

        public void AddBonus(ModifyType type, float value)
        {
            switch (type)
            {
                case ModifyType.FLAT_ADDITION:
                    AddToFlat(value);
                    return;

                case ModifyType.ADDITIVE:
                    AddToAdditive((int)value);
                    return;

                case ModifyType.MULTIPLY:
                    AddToMultiply(value);
                    return;

                case ModifyType.FIXED_TO:
                    AddFixedBonus(value);
                    return;
            }
        }

        public void RemoveBonus(ModifyType type, float value)
        {
            switch (type)
            {
                case ModifyType.FLAT_ADDITION:
                    AddToFlat(-value);
                    return;

                case ModifyType.ADDITIVE:
                    AddToAdditive((int)-value);
                    return;

                case ModifyType.MULTIPLY:
                    RemoveFromMultiply(value);
                    return;

                case ModifyType.FIXED_TO:
                    RemoveFixedBonus(value);
                    return;
            }
        }

        public void SetFlat(int value)
        {
            FlatModifier = value;
            isStatOutdated = true;
        }

        public void SetAdditive(int value)
        {
            AdditiveModifier = value;
            isStatOutdated = true;
        }

        private void AddFixedBonus(float value)
        {
            HasFixedModifier = true;
            FixedModifier = value;
        }

        private void RemoveFixedBonus(float value)
        {
            HasFixedModifier = false;
            FixedModifier = value;
        }

        private void AddToFlat(float value)
        {
            FlatModifier += value;
            isStatOutdated = true;
        }

        private void AddToAdditive(int value)
        {
            AdditiveModifier += value;
            isStatOutdated = true;
        }

        private void AddToMultiply(float value)
        {
            MultiplyModifiers.Add(value);
            UpdateCurrentMultiply();
            isStatOutdated = true;
        }

        private void RemoveFromMultiply(float value)
        {
            MultiplyModifiers.Remove(value);
            UpdateCurrentMultiply();
            isStatOutdated = true;
        }

        public void UpdateCurrentMultiply()
        {
            float mult = 1.0f;
            foreach (float i in MultiplyModifiers)
                mult *= 1f + i / 100f;
            CurrentMultiplier = mult;
        }

        public int CalculateStat(int stat)
        {
            return (int)CalculateStat((float)stat);
        }

        public float CalculateStat(float stat)
        {
            isStatOutdated = false;
            if (HasFixedModifier)
            {
                return FixedModifier;
            }
            return (stat + FlatModifier) * (1f + AdditiveModifier / 100f) * CurrentMultiplier;
        }
    }
}