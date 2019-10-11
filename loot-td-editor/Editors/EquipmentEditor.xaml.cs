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
            Dictionary<string, int> nameDict = new Dictionary<string, int>();
            foreach(EquipmentBase item in Equipments)
            {
                string chars = new String(item.IdName.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                if (!nameDict.ContainsKey(chars))
                    nameDict.Add(chars, 0);
                nameDict[chars]++;
                item.IdName = chars + nameDict[chars];
            }
            EquipList.ItemsSource = Equipments;
        }

        private void ReqButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (EquipmentBase equipment in Equipments)
            {
                EquipBaseFields.CalculateReqValues(equipment);
            }
        }

        private void ValueButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (EquipmentBase equipment in Equipments)
            {
                EquipBaseFields.CalculateArmorValues(equipment);
            }
        }

        private void CreateOtherArmorsButton_Click(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null || EquipList.SelectedItems.Count == 0)
                return;

            foreach (EquipmentBase item in EquipList.SelectedItems)
            {
                if (item.EquipSlot != EquipSlotType.BODY_ARMOR)
                    continue;

                EquipmentBase helm = Helpers.DeepClone(item);
                helm.IdName = helm.IdName.Replace("Armor", "Helm");
                helm.EquipSlot = EquipSlotType.HEADGEAR;
                EquipBaseFields.CalculateArmorValues(helm);
                EquipBaseFields.CalculateReqValues(helm);

                Equipments.Add(helm);

                EquipmentBase boots = Helpers.DeepClone(item);
                boots.IdName = boots.IdName.Replace("Armor", "Boots");
                boots.EquipSlot = EquipSlotType.BOOTS;
                EquipBaseFields.CalculateArmorValues(boots);
                EquipBaseFields.CalculateReqValues(boots);

                Equipments.Add(boots);

                EquipmentBase gloves = Helpers.DeepClone(item);
                gloves.IdName = gloves.IdName.Replace("Armor", "Gloves");
                gloves.EquipSlot = EquipSlotType.GLOVES;
                EquipBaseFields.CalculateArmorValues(gloves);
                EquipBaseFields.CalculateReqValues(gloves);

                Equipments.Add(gloves);
            }
        }

        private void CreateHybridStr_Click(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null || EquipList.SelectedItems.Count == 0)
                return;

            foreach (EquipmentBase item in EquipList.SelectedItems)
            {


                CreateHybrid(item,"Str","StrAgi", GroupType.STR_AGI_ARMOR);
                CreateHybrid(item, "Str", "StrInt", GroupType.STR_INT_ARMOR);
            }
        }

        private void CreateHybridAgi_Click(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null || EquipList.SelectedItems.Count == 0)
                return;

            foreach (EquipmentBase item in EquipList.SelectedItems)
            {


                CreateHybrid(item, "Agi", "IntAgi", GroupType.INT_AGI_ARMOR);
                CreateHybrid(item, "Agi", "StrAgi", GroupType.STR_AGI_ARMOR);
            }
        }

        private void CreateHybridInt_Click(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null || EquipList.SelectedItems.Count == 0)
                return;

            foreach (EquipmentBase item in EquipList.SelectedItems)
            {


                CreateHybrid(item, "Int", "StrInt", GroupType.STR_INT_ARMOR);
                CreateHybrid(item, "Int", "IntAgi", GroupType.INT_AGI_ARMOR);
            }
        }

        private void CreateHybrid(EquipmentBase item, string oldVal, string newVal, GroupType group)
        {
            EquipmentBase hybrid = Helpers.DeepClone(item);
            hybrid.IdName = hybrid.IdName.Replace(oldVal, newVal);
            if (Equipments.ToList().Find(x=>x.IdName == hybrid.IdName) != null)
            {
                return;
            }
            hybrid.Group = group;
            EquipBaseFields.CalculateArmorValues(hybrid);
            EquipBaseFields.CalculateReqValues(hybrid);

            Equipments.Add(hybrid);
        }
    }
}