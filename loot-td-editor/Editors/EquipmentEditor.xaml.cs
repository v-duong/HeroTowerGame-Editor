﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using loot_td;
using Newtonsoft.Json;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for EquipmentEditor.xaml
    /// </summary>
    public partial class EquipmentEditor : UserControl
    {
        public static float armorScaling = 10f;
        public static float shieldScaling = 15f;
        public static float dodgeRatingScaling = 8f;
        public static float resolveRatingScaling = 5.5f;
        public static float hybridMult = 0.5f;
        public static float willHybridMult = 0.75f;

        public static float mainAttrScaling = 2.5f;
        public static float subAttrScaling = 1.0f;
        public static float hybridFactor = 1.6f;
        public static int startingAttr = 10;

        public List<AffixBase> innatesList { get; set; }
        public List<EquipmentBase> Equipments;
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
                Equipments = new List<EquipmentBase>();
                EquipList.ItemsSource = Equipments;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Equipments = JsonConvert.DeserializeObject<List<EquipmentBase>>(json);
            EquipList.ItemsSource = Equipments;
            if (Equipments.Count >= 1)
                currentID = Equipments[Equipments.Count - 1].Id + 1;
            else
                currentID = 0;
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
                Id = currentID,
                Name = "UNTITLED NEW",
                InnateAffixId = -1
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
                Id = currentID,
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
            t.InnateAffixId = 0;
            InnateBox.SelectedIndex = 0;
        }

        private void HasInnateBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EquipmentBase t = (EquipmentBase)EquipList.SelectedItem;
            t.HasInnate = false;
            t.InnateAffixId = -1;
            InnateBox.SelectedIndex = -1;
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
                return (int)Math.Floor(scaling * droplevel);
            } else if (hybridtype == 1)
            {
                return (int)Math.Floor(droplevel * scaling * hybridMult);
            } else if (hybridtype == 2)
            {
                return (int)Math.Floor(droplevel * scaling * willHybridMult);
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
    }

}