﻿using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace loot_td_editor.Editors
{
    /// <summary>
    /// Interaction logic for EnemyEditor.xaml
    /// </summary>
    public partial class EnemyEditor : UserControl
    {
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }
        public IList<EffectType> EffectTypes { get { return Enum.GetValues(typeof(EffectType)).Cast<EffectType>().ToList<EffectType>(); } }

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

        public ObservableCollection<EnemyBase> EnemyBaseList;

        public void InitializeList()
        {
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\enemies\\enemies.editor.json";
            if (!System.IO.File.Exists(filePath))
            {
                EnemyBaseList = new ObservableCollection<EnemyBase>();
                EnemyListView.ItemsSource = EnemyBaseList;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            EnemyBaseList = JsonConvert.DeserializeObject<ObservableCollection<EnemyBase>>(json);

            foreach (EnemyBase k in EnemyBaseList)
            {
                if (k.IdName == null)
                    k.IdName = "";
            }

            EnemyListView.ItemsSource = EnemyBaseList;
        }

        public EnemyEditor()
        {
            InitializeComponent();
            InitializeList();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            EnemyBase temp = new EnemyBase
            {
                IdName = "UNTITLED NEW",
            };
            EnemyBaseList.Add(temp);
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (EnemyListView.SelectedItem == null)
                return;
            EnemyBase temp = Helpers.DeepClone((EnemyBase)EnemyListView.SelectedItem);

            EnemyBaseList.Add(temp);
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (EnemyListView.SelectedItem == null)
                return;

            string name = ((EnemyBase)EnemyListView.SelectedItem).IdName;

            MessageBoxResult res = System.Windows.MessageBox.Show("Delete " + name + "?", "Confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.No)
                return;

            EnemyBaseList.Remove((EnemyBase)EnemyListView.SelectedItem);
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
              (ListCollectionView)CollectionViewSource.GetDefaultView(EnemyListView.ItemsSource);

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

        private void Health_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (EnemyListView.SelectedItem == null)
                return;
            EnemyBase enemy = EnemyListView.SelectedItem as EnemyBase;
            HealthLabel1.Content = "Level " + enemy.Level;
            HealthValue1.Content = Helpers.EnemyHealthScalingFormula(enemy.Level) * enemy.HealthScaling;
            HealthValue2.Content = Helpers.EnemyHealthScalingFormula(100) * enemy.HealthScaling;
        }

        private void GroupComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            box.ItemsSource = GroupTypes.ToList();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            box.IsTextSearchEnabled = false;
            box.ItemsSource = BonusTypes.ToList();
        }
    }
}