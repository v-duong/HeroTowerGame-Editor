﻿using loot_td;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for EquipBaseFields.xaml
    /// </summary>
    public partial class EquipBaseFields : UserControl
    {
        public static float armorScaling = 3f;
        public static float shieldScaling = 4.5f;
        public static float dodgeRatingScaling = 2.3f;
        public static float resolveRatingScaling = 1.5f;
        public static float hybridMult = 0.5f;
        public static float willHybridMult = 0.75f;

        public static float mainAttrScaling = 0.9f;
        public static float subAttrScaling = 0.55f;
        public static float hybridFactor = 0.75f;
        public static int startingAttr = 5;

        public EquipmentBase SelectedBase
        {
            get { return (EquipmentBase)GetValue(SelectedBaseProperty); }
            set { SetValue(SelectedBaseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBase.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBaseProperty =
            DependencyProperty.Register("SelectedBase", typeof(EquipmentBase), typeof(EquipBaseFields));

        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList(); } }
        public IList<EquipSlotType> EquipSlotTypes { get { return Enum.GetValues(typeof(EquipSlotType)).Cast<EquipSlotType>().ToList(); } }

        public EquipBaseFields()
        {
            InitializeComponent();

            EquipSlotBox.ItemsSource = EquipSlotTypes;
            GroupTypeBox.ItemsSource = GroupTypes;
        }

        private void DPS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedBase == null)
                return;
            double dps = 0;
            dps = ((SelectedBase.MinDamage + SelectedBase.MaxDamage) / 2d) * SelectedBase.AttackSpeed;
            DPSLabel.Content = dps;
        }

        private void HasInnateBox_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedBase == null)
                return;
        }

        private void HasInnateBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectedBase == null)
                return;
            SelectedBase.InnateAffixId = null;
        }

        private void SetArmorValuesClick(object sender, RoutedEventArgs e)
        {
            if (SelectedBase == null)
                return;
            CalculateArmorValues(SelectedBase);
        }

        private void SetOffenseValuesClick(object sender, RoutedEventArgs e)
        {
        }

        private void SetReqValuesClick(object sender, RoutedEventArgs e)
        {
            if (SelectedBase == null)
                return;
            CalculateReqValues(SelectedBase);
        }

        public static int GetScaledDefense(int droplevel, int hybridtype, float scaling)
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

        public static void CalculateArmorValues(EquipmentBase b)
        {
            b.Armor = 0;
            b.Shield = 0;
            b.DodgeRating = 0;
            b.ResolveRating = 0;
            b.SellValue = 0;

            float scalingMulti = 1;
            /*
            if (ScalingMult.Value != null)
                scalingMulti = (float)ScalingMult.Value;
                */

            switch (b.EquipSlot)
            {
                case EquipSlotType.OFF_HAND:
                    scalingMulti = 0.8f;
                    break;

                case EquipSlotType.HEADGEAR:
                    scalingMulti = 0.4f;
                    break;

                case EquipSlotType.GLOVES:
                    scalingMulti = 0.25f;
                    break;

                case EquipSlotType.BOOTS:
                    scalingMulti = 0.3f;
                    break;

                default:
                    scalingMulti = 1f;
                    break;
            }

            switch (b.Group)
            {
                case GroupType.STR_ARMOR:
                case GroupType.STR_SHIELD:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 0, armorScaling) * scalingMulti);
                    break;

                case GroupType.INT_ARMOR:
                case GroupType.INT_SHIELD:
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 0, shieldScaling) * scalingMulti);
                    break;

                case GroupType.AGI_ARMOR:
                case GroupType.AGI_SHIELD:
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 0, dodgeRatingScaling) * scalingMulti);
                    break;

                case GroupType.STR_AGI_ARMOR:
                case GroupType.STR_AGI_SHIELD:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 1, armorScaling) * scalingMulti);
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 1, dodgeRatingScaling) * scalingMulti);
                    break;

                case GroupType.STR_WILL_ARMOR:
                case GroupType.STR_WILL_SHIELD:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 2, armorScaling) * scalingMulti);
                    b.ResolveRating = (int)(ResolveRatingValue(b.DropLevel) * scalingMulti);
                    break;

                case GroupType.STR_INT_ARMOR:
                case GroupType.STR_INT_SHIELD:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 1, armorScaling) * scalingMulti);
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 1, shieldScaling) * scalingMulti);
                    break;

                case GroupType.INT_AGI_ARMOR:
                case GroupType.INT_AGI_SHIELD:
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 1, shieldScaling) * scalingMulti);
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 0, dodgeRatingScaling) * scalingMulti);
                    break;

                case GroupType.INT_WILL_ARMOR:
                case GroupType.INT_WILL_SHIELD:
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 2, shieldScaling) * scalingMulti);
                    b.ResolveRating = (int)(ResolveRatingValue(b.DropLevel) * scalingMulti);
                    break;

                case GroupType.AGI_WILL_ARMOR:
                case GroupType.AGI_WILL_SHIELD:
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 2, dodgeRatingScaling) * scalingMulti);
                    b.ResolveRating = (int)(ResolveRatingValue(b.DropLevel) * scalingMulti);
                    break;

                default:
                    return;
            }
        }

        private static int ResolveRatingValue(int level)
        {
            return (int)(level * resolveRatingScaling);
        }

        public static void CalculateReqValues(EquipmentBase b)
        {
            b.StrengthReq = 0;
            b.AgilityReq = 0;
            b.IntelligenceReq = 0;
            b.WillReq = 0;

            float scalingMulti = 1;
            /*
            if (ScalingMult.Value != null)
                scalingMulti = (float)ScalingMult.Value;
                */

            switch (b.EquipSlot)
            {
                case EquipSlotType.OFF_HAND:
                    scalingMulti = 0.85f;
                    break;

                case EquipSlotType.HEADGEAR:
                    scalingMulti = 0.65f;
                    break;

                case EquipSlotType.GLOVES:
                    scalingMulti = 0.4f;
                    break;

                case EquipSlotType.BOOTS:
                    scalingMulti = 0.5f;
                    break;

                default:
                    scalingMulti = 1f;
                    break;
            }

            int mainreq = (int)Math.Floor((Math.Pow(b.DropLevel, 1.13) * mainAttrScaling + startingAttr) * scalingMulti);
            int hybridmain = (int)Math.Floor((Math.Pow(b.DropLevel, 1.13) * mainAttrScaling * hybridFactor + startingAttr) * scalingMulti);
            int hybridsub = (int)Math.Floor((Math.Pow(b.DropLevel, 1.13) * subAttrScaling + startingAttr) * scalingMulti);

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

                case GroupType.ONE_HANDED_SWORD:
                    b.StrengthReq = hybridmain;
                    b.AgilityReq = (int)(hybridmain * 0.8f);
                    break;

                case GroupType.ONE_HANDED_AXE:
                    b.StrengthReq = (int)(mainreq);
                    b.AgilityReq = (int)(hybridmain * 0.2f);
                    break;

                case GroupType.BOW:
                    b.AgilityReq = (int)(mainreq * 1.1f);
                    break;

                case GroupType.SPEAR:
                    b.StrengthReq = (int)(mainreq * 1.1f);
                    b.AgilityReq = (int)(hybridmain * 0.4f);
                    break;

                case GroupType.TWO_HANDED_SWORD:
                    b.StrengthReq = (int)(mainreq * 1.05f);
                    b.AgilityReq = (int)(hybridmain * 0.5f);
                    break;

                case GroupType.TWO_HANDED_AXE:
                    b.StrengthReq = (int)(mainreq * 1.25f);
                    break;

                case GroupType.ONE_HANDED_GUN:
                    b.AgilityReq = (int)(mainreq);
                    break;

                case GroupType.TWO_HANDED_GUN:
                    b.AgilityReq = (int)(mainreq * 1.1f);
                    break;

                case GroupType.ONE_HANDED_MACE:
                    b.StrengthReq = (int)(mainreq);
                    break;

                case GroupType.TWO_HANDED_MACE:
                    b.StrengthReq = (int)(mainreq * 1.1f);
                    break;

                case GroupType.WAND:
                    b.IntelligenceReq = (int)(mainreq);
                    break;

                case GroupType.STAFF:
                    b.IntelligenceReq = (int)(mainreq);
                    b.StrengthReq = (int)(hybridmain * 0.3f);
                    break;

                case GroupType.STR_SHIELD:
                    b.StrengthReq = (int)(mainreq * 0.8f);
                    break;

                case GroupType.INT_SHIELD:
                    b.IntelligenceReq = (int)(mainreq * 0.8f);
                    break;

                case GroupType.AGI_SHIELD:
                    b.AgilityReq = (int)(mainreq * 0.8f);
                    break;

                case GroupType.STR_AGI_SHIELD:
                    b.StrengthReq = (int)(hybridmain * 0.8f);
                    b.AgilityReq = (int)(hybridmain * 0.8f);
                    break;

                case GroupType.STR_WILL_SHIELD:
                    b.StrengthReq = (int)(hybridmain * 0.8f);
                    b.WillReq = (int)(hybridsub * 0.8f);
                    break;

                case GroupType.STR_INT_SHIELD:
                    b.StrengthReq = (int)(hybridmain * 0.8f);
                    b.IntelligenceReq = (int)(hybridmain * 0.8f);
                    break;

                case GroupType.INT_AGI_SHIELD:
                    b.IntelligenceReq = (int)(hybridmain * 0.8f);
                    b.AgilityReq = (int)(hybridmain * 0.8f);
                    break;

                case GroupType.INT_WILL_SHIELD:
                    b.IntelligenceReq = (int)(hybridmain * 0.8f);
                    b.WillReq = (int)(hybridsub * 0.8f);
                    break;

                case GroupType.AGI_WILL_SHIELD:
                    b.AgilityReq = (int)(hybridmain * 0.8f);
                    b.WillReq = (int)(hybridsub * 0.8f);
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
            if (SelectedBase == null)
                return;
            CalculateArmorValues(SelectedBase);
        }

        private void UseScalingBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ScalingMult != null)
                ScalingMult.Value = 1;

            if (SelectedBase == null)
                return;

            CalculateArmorValues(SelectedBase);
            CalculateReqValues(SelectedBase);
        }

        private void UseScalingBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ScalingMult.Value = 1;
        }

        private void GroupTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedBase == null)
                return;
            ComboBox box = sender as ComboBox;
            switch ((GroupType)box.SelectedItem)
            {
                case GroupType.STR_SHIELD:
                case GroupType.AGI_SHIELD:
                case GroupType.SHIELD:
                case GroupType.INT_SHIELD:
                case GroupType.WILL_SHIELD:
                case GroupType.STR_INT_SHIELD:
                case GroupType.STR_AGI_SHIELD:
                case GroupType.STR_WILL_SHIELD:
                case GroupType.INT_AGI_SHIELD:
                case GroupType.INT_WILL_SHIELD:
                case GroupType.AGI_WILL_SHIELD:
                    AttackSpeedLabel.Content = "Shield Prot";
                    CritChanceLabel.Content = "Shield Block";
                    break;

                default:
                    AttackSpeedLabel.Content = "AttackSpeed";
                    CritChanceLabel.Content = "CritChance";
                    break;
            }
        }
    }
}