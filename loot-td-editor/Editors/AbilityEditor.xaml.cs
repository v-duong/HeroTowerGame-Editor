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
using Xceed.Wpf.Toolkit;

namespace loot_td_editor.Editors
{
    /// <summary>
    /// Interaction logic for AbilityEditor.xaml
    /// </summary>
    public partial class AbilityEditor : UserControl
    {
        public ObservableCollection<AbilityBase> Abilities;
        private int currentID;
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }

        public void InitializeList()
        {
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\abilities\\abilities.editor.json";
            Debug.WriteLine("Initialized abilities");
            if (!System.IO.File.Exists(filePath))
            {
                Abilities = new ObservableCollection<AbilityBase>();
                AbilitiesList.ItemsSource = Abilities;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Abilities = JsonConvert.DeserializeObject<ObservableCollection<AbilityBase>>(json);

            foreach (AbilityBase k in Abilities)
            {
                if (k.IdName == null)
                    k.IdName = "";
                if (k.LinkedAbility == null)
                    k.LinkedAbility = new LinkedAbilityData();

                Debug.Print(k.AppliedEffects.ToString());
            }

            AbilitiesList.ItemsSource = Abilities;
        }

        public AbilityEditor()
        {
            InitializeComponent();
            InitializeList();
            GroupList.ItemsSource = GroupTypes;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(GroupList.ItemsSource);
            view.Filter = TagFilter;
            LinkedAbilityList.ItemsSource = Abilities;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AbilityBase temp = new AbilityBase
            {
                IdName = "UNTITLED NEW",
            };
            Abilities.Add(temp);
            //AbilitiesList.Items.Refresh();
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = Helpers.DeepClone((AbilityBase)AbilitiesList.SelectedItem);

            Abilities.Add(temp);
            //AbilitiesList.Items.Refresh();
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (AbilitiesList.SelectedItem == null)
                return;

            string name = ((AbilityBase)AbilitiesList.SelectedItem).IdName;

            MessageBoxResult res = System.Windows.MessageBox.Show("Delete " + name + "?", "Confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.No)
                return;

            Abilities.Remove((AbilityBase)AbilitiesList.SelectedItem);
            //ArchetypesList.Items.Refresh();
        }

        private void GroupAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupList.SelectedItem == null || AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = (AbilityBase)AbilitiesList.SelectedItem;
            if (!temp.GroupTypes.Contains((GroupType)GroupList.SelectedItem))
                temp.GroupTypes.Add((GroupType)GroupList.SelectedItem);
            else
                return;
            //GroupTagList.Items.Refresh();
        }

        private void GroupRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupTagList.SelectedItem == null || AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = (AbilityBase)AbilitiesList.SelectedItem;
            temp.GroupTypes.Remove((GroupType)GroupTagList.SelectedItem);
            //GroupTagList.Items.Refresh();
        }

        private void RestrictAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupList.SelectedItem == null || AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = (AbilityBase)AbilitiesList.SelectedItem;
            if (!temp.RequiredRestrictions.Contains((GroupType)GroupList.SelectedItem))
                temp.RequiredRestrictions.Add((GroupType)GroupList.SelectedItem);
            else
                return;
            //GroupTagList.Items.Refresh();
        }

        private void RestrictRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (RestrictionList.SelectedItem == null || AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = (AbilityBase)AbilitiesList.SelectedItem;
            temp.RequiredRestrictions.Remove((GroupType)RestrictionList.SelectedItem);
            //GroupTagList.Items.Refresh();
        }

        private void SingleRestrictAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupList.SelectedItem == null || AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = (AbilityBase)AbilitiesList.SelectedItem;
            if (!temp.SingleRequireRestrictions.Contains((GroupType)GroupList.SelectedItem))
                temp.SingleRequireRestrictions.Add((GroupType)GroupList.SelectedItem);
            else
                return;
            //GroupTagList.Items.Refresh();
        }

        private void SingleRestrictRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (SingleRestrictionList.SelectedItem == null || AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = (AbilityBase)AbilitiesList.SelectedItem;
            temp.SingleRequireRestrictions.Remove((GroupType)SingleRestrictionList.SelectedItem);
            //GroupTagList.Items.Refresh();
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(GroupList.ItemsSource).Refresh();
        }

        private bool TagFilter(object item)
        {
            if (item == null)
                return false;
            string s = (item as GroupType?).ToString();

            if (String.IsNullOrEmpty(FilterBox.Text))
            {
                return true;
            }
            else
            {
                return s.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        private void WeaponBaseMult_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DoubleUpDown d = sender as DoubleUpDown;

            WeaponMultLabel.Content = d.Value;
            WeaponSumLabel.Content = d.Value + WeaponScaleInput.Value * 50;
        }

        private void DoubleUpDown_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DoubleUpDown d = sender as DoubleUpDown;

            WeaponScaleLabel.Content = d.Value * 50;
            WeaponSumLabel.Content = WeaponBaseMultInput.Value + d.Value * 50;
        }

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            ListSortDirection direction;

            if (e.OriginalSource is GridViewColumnHeader headerClicked)
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
              (ListCollectionView)CollectionViewSource.GetDefaultView(AbilitiesList.ItemsSource);

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

        private void AbilityTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AbilitiesList.SelectedItem == null)
                return;
            AbilityBase b = AbilitiesList.SelectedItem as AbilityBase;
            if (b.AbilityType == AbilityType.ATTACK)
            {
                AttacksPerSecBox.IsEnabled = false;
                BaseCriticalBox.IsEnabled = false;
                FlatDamageMultiBox.IsEnabled = false;
            } else
            {
                AttacksPerSecBox.IsEnabled = true;
                BaseCriticalBox.IsEnabled = true;
                FlatDamageMultiBox.IsEnabled = true;
            }

            if (b.AbilityShotType.ToString().Contains("ARC"))
            {
                ProjSpreadLabel.Content = "Arc Angle";
            } else
            {
                ProjSpreadLabel.Content = "Proj Sprd Angle";
            }

            if (b.AbilityShotType.ToString().Contains("FORWARD_MOVING"))
            {
                ProjLifetimeLabel.Content = "Move Duration";
            } else
            {
                ProjLifetimeLabel.Content = "Proj Lifetime";
            }
        }

        private void AbilitiesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AbilitiesList.SelectedItem == null)
                return;

            AbilityBase abilityBase = AbilitiesList.SelectedItem as AbilityBase;

            string s = "";
            foreach(ArchetypeBase archetypeBase in Helpers.archetypeEditor.Archetypes)
            {
                if (archetypeBase.NodeList.ToList().FindAll(x=>x.Type == NodeType.ABILITY && x.AbilityId == abilityBase.IdName).Any())
                {
                    s += archetypeBase.IdName + "\n";
                    continue;
                }
            }
            ArchetypeUseList.Text = s;
        }
    }
}