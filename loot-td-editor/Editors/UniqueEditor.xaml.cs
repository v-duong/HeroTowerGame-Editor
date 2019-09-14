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
            Debug.WriteLine(filePath);
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
                BaseFields.CalculateReqValues(equipment);
            }
        }

        private void ValueButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (UniqueBase equipment in Uniques)
            {
                BaseFields.CalculateArmorValues(equipment);
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
}