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
        public static float armorScaling = 3f;
        public static float shieldScaling = 4.5f;
        public static float dodgeRatingScaling = 2.3f;
        public static float resolveRatingScaling = 1.12f;
        public static float hybridMult = 0.5f;
        public static float willHybridMult = 0.75f;

        public static float mainAttrScaling = 2.5f;
        public static float subAttrScaling = 1.0f;
        public static float hybridFactor = 1.6f;
        public static int startingAttr = 10;

        public ObservableCollection<AffixBase> innatesList { get; set; }
        public ObservableCollection<EquipmentBase> Equipments;
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }
        public IList<EquipSlotType> EquipSlotTypes { get { return Enum.GetValues(typeof(EquipSlotType)).Cast<EquipSlotType>().ToList<EquipSlotType>(); } }

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
            EquipSlotBox.ItemsSource = EquipSlotTypes;
            GroupTypeBox.ItemsSource = GroupTypes;
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
            EquipList.Items.Refresh();
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            EquipmentBase temp = new EquipmentBase((EquipmentBase)EquipList.SelectedItem)
            {

            };
            Equipments.Add(temp);
            EquipList.Items.Refresh();
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipList.SelectedItem == null)
                return;
            Equipments.Remove((EquipmentBase)EquipList.SelectedItem);
            EquipList.Items.Refresh();
        }

        private void HasInnateBox_Checked(object sender, RoutedEventArgs e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;
            t.HasInnate = true;
        }

        private void HasInnateBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;
            t.HasInnate = false;
            t.InnateAffixId = null;
        }

        private void SetArmorValuesClick(object sender, RoutedEventArgs e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;
            if (t == null)
                return;
            CalculateArmorValues(t);
        }

        private void SetOffenseValuesClick(object sender, RoutedEventArgs e)
        {
        }

        private void SetReqValuesClick(object sender, RoutedEventArgs e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;
            if (t == null)
                return;
            CalculateReqValues(t);
        }

        private int GetScaledDefense(int droplevel, int hybridtype, float scaling)
        {
            if (hybridtype == 0)
            {
                return (int)Math.Floor(Helpers.EquipScalingFormula(droplevel) * scaling + 10);
            }
            else if (hybridtype == 1)
            {
                return (int)Math.Floor((Helpers.EquipScalingFormula(droplevel) * scaling + 10) * hybridMult);
            }
            else if (hybridtype == 2)
            {
                return (int)Math.Floor((Helpers.EquipScalingFormula(droplevel) * scaling + 10) * willHybridMult);
            }
            return 0;
        }

        private void CalculateArmorValues(EquipmentBase b)
        {
            b.Armor = 0;
            b.Shield = 0;
            b.DodgeRating = 0;
            b.ResolveRating = 0;
            b.SellValue = 0;
            switch (b.Group)
            {
                case GroupType.STR_ARMOR:
                    b.Armor = GetScaledDefense(b.DropLevel, 0, armorScaling);
                    break;

                case GroupType.INT_ARMOR:
                    b.Shield = GetScaledDefense(b.DropLevel, 0, shieldScaling);
                    break;

                case GroupType.AGI_ARMOR:
                    b.DodgeRating = GetScaledDefense(b.DropLevel, 0, dodgeRatingScaling);
                    break;

                case GroupType.STR_AGI_ARMOR:
                    b.Armor = GetScaledDefense(b.DropLevel, 1, armorScaling);
                    b.DodgeRating = GetScaledDefense(b.DropLevel, 1, dodgeRatingScaling);
                    break;

                case GroupType.STR_WILL_ARMOR:
                    b.Armor = GetScaledDefense(b.DropLevel, 2, armorScaling);
                    b.ResolveRating = GetScaledDefense(b.DropLevel, 0, resolveRatingScaling);
                    break;

                case GroupType.STR_INT_ARMOR:
                    b.Armor = GetScaledDefense(b.DropLevel, 1, armorScaling);
                    b.Shield = GetScaledDefense(b.DropLevel, 1, shieldScaling);
                    break;

                case GroupType.INT_AGI_ARMOR:
                    b.Shield = GetScaledDefense(b.DropLevel, 1, shieldScaling);
                    b.DodgeRating = GetScaledDefense(b.DropLevel, 0, dodgeRatingScaling);
                    break;

                case GroupType.INT_WILL_ARMOR:
                    b.Shield = GetScaledDefense(b.DropLevel, 2, shieldScaling);
                    b.ResolveRating = GetScaledDefense(b.DropLevel, 0, resolveRatingScaling);
                    break;

                case GroupType.AGI_WILL_ARMOR:
                    b.DodgeRating = GetScaledDefense(b.DropLevel, 2, dodgeRatingScaling);
                    b.ResolveRating = GetScaledDefense(b.DropLevel, 0, resolveRatingScaling);
                    break;

                default:
                    return;
            }
        }

        private void CalculateReqValues(EquipmentBase b)
        {
            b.StrengthReq = 0;
            b.AgilityReq = 0;
            b.IntelligenceReq = 0;
            b.WillReq = 0;

            int mainreq = (int)Math.Floor(b.DropLevel * mainAttrScaling + startingAttr);
            int hybridmain = (int)Math.Floor(b.DropLevel * mainAttrScaling * hybridFactor + startingAttr);
            int hybridsub = (int)Math.Floor(b.DropLevel * subAttrScaling + startingAttr);

            switch (b.Group)
            {
                case GroupType.STR_ARMOR:
                    b.StrengthReq = mainreq;
                    break;

                case GroupType.INT_ARMOR:
                    b.IntelligenceReq = mainreq;
                    break;

                case GroupType.AGI_ARMOR:
                    b.AgilityReq = mainreq;
                    break;

                case GroupType.STR_AGI_ARMOR:
                    b.StrengthReq = hybridmain;
                    b.AgilityReq = hybridmain;
                    break;

                case GroupType.STR_WILL_ARMOR:
                    b.StrengthReq = hybridmain;
                    b.WillReq = hybridsub;
                    break;

                case GroupType.STR_INT_ARMOR:
                    b.StrengthReq = hybridmain;
                    b.IntelligenceReq = hybridmain;
                    break;

                case GroupType.INT_AGI_ARMOR:
                    b.IntelligenceReq = hybridmain;
                    b.AgilityReq = hybridmain;
                    break;

                case GroupType.INT_WILL_ARMOR:
                    b.IntelligenceReq = hybridmain;
                    b.WillReq = hybridsub;
                    break;

                case GroupType.AGI_WILL_ARMOR:
                    b.AgilityReq = hybridmain;
                    b.WillReq = hybridsub;
                    break;

                default:
                    return;
            }
        }

        private void EquipSlotBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void InnateBox_DropDownOpened(object sender, EventArgs e)
        {
        }

        private void ScalingMult_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;
            if (t == null)
                return;
            CalculateArmorValues(t);
        }

        private void UseScalingBox_Checked(object sender, RoutedEventArgs e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;

            if (ScalingMult != null)
                ScalingMult.Value = 1;

            if (t == null)
                return;

            CalculateArmorValues(t);
            CalculateReqValues(t);
        }

        private void UseScalingBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;
            ScalingMult.Value = 1;
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
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(EquipList.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
    }
}