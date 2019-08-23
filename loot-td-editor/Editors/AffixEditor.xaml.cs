﻿using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    ///

    public partial class AffixEditor : UserControl
    {
        public ObservableCollection<AffixBase> Affixes;
        
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }
        private IList<BonusType> _bonusTypes;
        public IList<BonusType> BonusTypes
        {
            get
            {
                if (_bonusTypes == null)
                    _bonusTypes = Enum.GetValues(typeof(BonusType)).Cast<BonusType>().ToList<BonusType>();
                return _bonusTypes;
            }
        }

        private int currentID = 0;

        private string currentSearchString = string.Empty;

        public string AffixProp
        {
            get { return (string)GetValue(AffixPropProperty); }
            set { SetValue(AffixPropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AffixProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AffixPropProperty =
            DependencyProperty.Register("AffixProp", typeof(string), typeof(AffixEditor), new PropertyMetadata("", new PropertyChangedCallback(OnPropChange)));

        public static void OnPropChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AffixEditor a = d as AffixEditor;
            a.InitializeAffixes();
        }

        public AffixType affixContext;
        private bool isContextSet = false;

        public void InitializeAffixes()
        {
            if (!isContextSet)
            {
                Enum.TryParse(AffixProp, out AffixType aff);
                affixContext = aff;
            }
            string fileName;
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;
            switch (affixContext)
            {
                case AffixType.PREFIX:
                    fileName = "prefix.json";
                    break;

                case AffixType.SUFFIX:
                    fileName = "suffix.json";
                    break;

                case AffixType.ENCHANTMENT:
                    fileName = "enchantment.json";
                    break;

                case AffixType.INNATE:
                    fileName = "innate.json";
                    break;

                case AffixType.MONSTERMOD:
                    fileName = "monstermod.json";
                    break;

                default:
                    return;
            }
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\affixes\\" + fileName;
            Debug.WriteLine("Initialized " + fileName);
            if (!System.IO.File.Exists(filePath))
            {
                Affixes = new ObservableCollection<AffixBase>();
                AffixesList.ItemsSource = Affixes;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Affixes = JsonConvert.DeserializeObject<ObservableCollection<AffixBase>>(json);

            foreach (AffixBase k in Affixes)
            {
                if (k.IdName == null)
                    k.IdName = "";
            }
            AffixesList.ItemsSource = Affixes;
        }

        public AffixEditor()
        {
            InitializeComponent();
            GroupList.ItemsSource = GroupTypes;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(GroupList.ItemsSource);
            view.Filter = TagFilter;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isContextSet)
            {
                Enum.TryParse(AffixProp, out AffixType aff);
                affixContext = aff;
            }
            AffixBase temp = new AffixBase
            {
                IdName = "UNTITLED NEW",
                AffixType = affixContext,
                Tier = 1
            };
            Affixes.Add(temp);
            //AffixesList.Items.Refresh();
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            AffixBase temp = new AffixBase((AffixBase)AffixesList.SelectedItem)
            {
            };
            Affixes.Add(temp);
            //AffixesList.Items.Refresh();
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            Affixes.Remove((AffixBase)AffixesList.SelectedItem);
            //AffixesList.Items.Refresh();
        }

        private void AddBonusButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            AffixBonus b = new AffixBonus();
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            temp.AffixBonuses.Add(b);
            //BonusGrid.Items.Refresh();
            CountText.Text = "Count: " + temp.GetBonusCountString;
        }

        private void RemoveBonusButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            temp.AffixBonuses.Remove((AffixBonus)BonusGrid.SelectedItem);
            //BonusGrid.Items.Refresh();
            CountText.Text = "Count: " + temp.GetBonusCountString;
        }

        private void BonusGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //AffixesList.Items.Refresh();
        }


        private void GroupAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupList.SelectedItem == null || AffixesList.SelectedItem == null)
                return;
            var temp = AffixesList.SelectedItems;
            foreach (AffixBase affix in temp)
            {
                if (!affix.GroupTypes.Contains((GroupType)GroupList.SelectedItem))
                    affix.GroupTypes.Add((GroupType)GroupList.SelectedItem);
            }
            GroupTagList.Items.Refresh();
        }

        private void GroupRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupTagList.SelectedItem == null || AffixesList.SelectedItem == null)
                return;
            var temp = AffixesList.SelectedItems;
            foreach (AffixBase affix in temp)
            {
                affix.GroupTypes.Remove((GroupType)GroupTagList.SelectedItem);
            }
            GroupTagList.Items.Refresh();
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
              (ListCollectionView)CollectionViewSource.GetDefaultView(AffixesList.ItemsSource);

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

        private void CopyWeight_Click(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;

            AffixBase first = (AffixBase)AffixesList.SelectedItems[0];
            ObservableCollection<AffixWeight> firstList = first.SpawnWeight;

            foreach (AffixBase b in AffixesList.SelectedItems)
            {
                if (b == first)
                    continue;
                b.SpawnWeight = new ObservableCollection<AffixWeight>();
                foreach (AffixWeight w in first.SpawnWeight)
                {
                    b.SpawnWeight.Add(w.CloneWeight());
                }
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            box.IsTextSearchEnabled = false;
            box.ItemsSource = BonusTypes.ToList();
            var view = (ListCollectionView)CollectionViewSource.GetDefaultView(box.ItemsSource);
        }

    }

    public class BonusDataValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            BindingGroup g = (BindingGroup)value;

            foreach (AffixBonus x in g.Items)
            {
                if (x.MinValue > x.MaxValue)
                    return new ValidationResult(false, "MinValue cannot be higher than MaxValue");
            }

            return ValidationResult.ValidResult;
        }
    }
}