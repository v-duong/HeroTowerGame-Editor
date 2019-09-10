using System;
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
using loot_td_editor;
using loot_td;

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
            SelectedBase.HasInnate = true;
        }

        private void HasInnateBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectedBase == null)
                return;
            SelectedBase.HasInnate = false;
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

        public void CalculateArmorValues(EquipmentBase b)
        {
            float scalingMulti = 1;
            if (ScalingMult.Value != null)
                scalingMulti = (float)ScalingMult.Value;
            b.Armor = 0;
            b.Shield = 0;
            b.DodgeRating = 0;
            b.ResolveRating = 0;
            b.SellValue = 0;
            switch (b.Group)
            {
                case GroupType.STR_ARMOR:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 0, armorScaling) * scalingMulti);
                    break;

                case GroupType.INT_ARMOR:
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 0, shieldScaling) * scalingMulti);
                    break;

                case GroupType.AGI_ARMOR:
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 0, dodgeRatingScaling) * scalingMulti);
                    break;

                case GroupType.STR_AGI_ARMOR:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 1, armorScaling) * scalingMulti);
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 1, dodgeRatingScaling) * scalingMulti);
                    break;

                case GroupType.STR_WILL_ARMOR:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 2, armorScaling) * scalingMulti);
                    b.ResolveRating = (int)(ResolveRatingValue(b.DropLevel) * scalingMulti);
                    break;

                case GroupType.STR_INT_ARMOR:
                    b.Armor = (int)(GetScaledDefense(b.DropLevel, 1, armorScaling) * scalingMulti);
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 1, shieldScaling) * scalingMulti);
                    break;

                case GroupType.INT_AGI_ARMOR:
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 1, shieldScaling) * scalingMulti);
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 0, dodgeRatingScaling) * scalingMulti);
                    break;

                case GroupType.INT_WILL_ARMOR:
                    b.Shield = (int)(GetScaledDefense(b.DropLevel, 2, shieldScaling) * scalingMulti);
                    b.ResolveRating = (int)(ResolveRatingValue(b.DropLevel) * scalingMulti);
                    break;

                case GroupType.AGI_WILL_ARMOR:
                    b.DodgeRating = (int)(GetScaledDefense(b.DropLevel, 2, dodgeRatingScaling) * scalingMulti);
                    b.ResolveRating = (int)(ResolveRatingValue(b.DropLevel) * scalingMulti);
                    break;

                default:
                    return;
            }
        }

        private int ResolveRatingValue(int level)
        {
            return (int)(level * resolveRatingScaling);
        }

        public void CalculateReqValues(EquipmentBase b)
        {
            b.StrengthReq = 0;
            b.AgilityReq = 0;
            b.IntelligenceReq = 0;
            b.WillReq = 0;

            int mainreq = (int)Math.Floor(Math.Pow(b.DropLevel, 1.13) * mainAttrScaling + startingAttr);
            int hybridmain = (int)Math.Floor(Math.Pow(b.DropLevel, 1.13) * mainAttrScaling * hybridFactor + startingAttr);
            int hybridsub = (int)Math.Floor(Math.Pow(b.DropLevel, 1.13) * subAttrScaling + startingAttr);

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
                case GroupType.ONE_HANDED_GUN:
                    b.AgilityReq = (int)(mainreq);
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
    }
}
