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
    /// <summary>
    /// Interaction logic for EquipmentEditor.xaml
    /// </summary>
    public partial class EquipmentEditor : UserControl
    {

        public ObservableCollection<AffixBase> innatesList { get; set; }
        public ObservableCollection<EquipmentBase> Equipments;
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList(); } }
        public IList<EquipSlotType> EquipSlotTypes { get { return Enum.GetValues(typeof(EquipSlotType)).Cast<EquipSlotType>().ToList(); } }

        private int currentID = 0;

        public string EquipProp
        {
            get { return (string)GetValue(EquipPropProperty); }
            set { SetValue(EquipPropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AffixProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EquipPropProperty =
            DependencyProperty.Register("EquipProp", typeof(string), typeof(EquipmentEditor), new PropertyMetadata("", new PropertyChangedCallback(OnPropChange)));

        public static void OnPropChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EquipmentEditor a = d as EquipmentEditor;
            a.InitializeEquipments();
        }

        public void InitializeEquipments()
        {
            string fileName = EquipProp + ".json";
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;

            string filePath = Properties.Settings.Default.JsonLoadPath + "\\items\\" + fileName;
            Debug.WriteLine(filePath);
            if (!System.IO.File.Exists(filePath))
            {
                Equipments = new ObservableCollection<EquipmentBase>();
                EquipList.ItemsSource = Equipments;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Equipments = JsonConvert.DeserializeObject<ObservableCollection<EquipmentBase>>(json);

            foreach (EquipmentBase k in Equipments)
            {
                if (k.Description == null)
                    k.Description = "";
            }

            EquipList.ItemsSource = Equipments;
        }

        public EquipmentEditor()
        {
            InitializeComponent();
        }

        public void AddButtonClick()
        {
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            EquipmentBase temp = new EquipmentBase
            {
                IdName = "UNTITLED NEW",
                InnateAffixId = null
            };
            Equipments.Add(temp);
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            EquipmentBase temp = Helpers.DeepClone((EquipmentBase)EquipList.SelectedItem);
            Equipments.Add(temp);
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;

            string name = ((EquipmentBase)EquipList.SelectedItem).IdName;

            MessageBoxResult res = System.Windows.MessageBox.Show("Delete " + name + "?", "Confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.No)
                return;

            Equipments.Remove((EquipmentBase)EquipList.SelectedItem);
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
            Equipments = new ObservableCollection<EquipmentBase>(Equipments.OrderBy(x => x.IdName, new NaturalStringComparer2()));
            EquipList.ItemsSource = Equipments;
        }

        private void ReqButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (EquipmentBase equipment in Equipments)
            {
                BaseFields.CalculateReqValues(equipment);
            }
        }

        private void ValueButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (EquipmentBase equipment in Equipments)
            {
                BaseFields.CalculateArmorValues(equipment);
            }
        }
    }
}