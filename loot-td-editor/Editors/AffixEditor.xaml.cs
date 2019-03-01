using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public List<AffixBase> Affixes;
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }

        private int currentID = 0;

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

                default:
                    return;
            }
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\affixes\\" + fileName;
            Debug.WriteLine("Initialized " + fileName);
            if (!System.IO.File.Exists(filePath))
            {
                Affixes = new List<AffixBase>();
                AffixesList.ItemsSource = Affixes;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Affixes = JsonConvert.DeserializeObject<List<AffixBase>>(json);
            AffixesList.ItemsSource = Affixes;
            if (Affixes.Count >= 1)
                currentID = Affixes[Affixes.Count - 1].Id + 1;
            else
                currentID = 0;
        }

        public AffixEditor()
        {
            InitializeComponent();
            GroupList.ItemsSource = GroupTypes;
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
                Id = currentID,
                Name = "UNTITLED NEW",
                AffixType = affixContext,
                Tier = 1
            };
            Affixes.Add(temp);
            AffixesList.Items.Refresh();
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            AffixBase temp = new AffixBase((AffixBase)AffixesList.SelectedItem)
            {
                Id = currentID,
            };
            Affixes.Add(temp);
            AffixesList.Items.Refresh();
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            Affixes.Remove((AffixBase)AffixesList.SelectedItem);
            AffixesList.Items.Refresh();
        }

        private void AddBonusButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            AffixBonus b = new AffixBonus();
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            temp.AffixBonuses.Add(b);
            BonusGrid.Items.Refresh();
            CountText.Text = "Count: " + temp.GetBonusCountString;
        }

        private void RemoveBonusButtonClick(object sender, RoutedEventArgs e)
        {
            if (AffixesList.SelectedItem == null)
                return;
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            temp.AffixBonuses.Remove((AffixBonus)BonusGrid.SelectedItem);
            BonusGrid.Items.Refresh();
            CountText.Text = "Count: " + temp.GetBonusCountString;
        }

        private void BonusGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            AffixesList.Items.Refresh();
        }

        private void BonusGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            AffixesList.Items.Refresh();
        }

        private void AddWeightButtonClick(object sender, RoutedEventArgs e)
        {
            SpawnWeightEntryWindow s = new SpawnWeightEntryWindow();
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            if (temp == null)
                return;
            s.dic = temp.SpawnWeight;
            s.ShowDialog();
            WeightGrid.Items.Refresh();
        }

        private void EditWeightButtonClick(object sender, RoutedEventArgs e)
        {
            if (WeightGrid.SelectedItem == null)
                return;
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            if (temp == null)
                return;

            SpawnWeightEntryWindow s = new SpawnWeightEntryWindow((AffixWeight)WeightGrid.SelectedItem)
            {
                dic = temp.SpawnWeight
            };
            s.ShowDialog();
            WeightGrid.Items.Refresh();
        }

        private void RemoveWeightButtonClick(object sender, RoutedEventArgs e)
        {
            if (WeightGrid.SelectedItem == null)
                return;
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            temp.SpawnWeight.Remove((AffixWeight)WeightGrid.SelectedItem);
            WeightGrid.Items.Refresh();
        }

        private void GroupAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupList.SelectedItem == null)
                return;
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            if (!temp.GroupTypes.Contains((GroupType)GroupList.SelectedItem))
                temp.GroupTypes.Add((GroupType)GroupList.SelectedItem);
            else
                return;
            GroupTagList.Items.Refresh();
        }

        private void GroupRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (GroupTagList.SelectedItem == null)
                return;
            AffixBase temp = (AffixBase)AffixesList.SelectedItem;
            temp.GroupTypes.Remove((GroupType)GroupTagList.SelectedItem);
            GroupTagList.Items.Refresh();
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
                if (x.ModifyType == ModifyType.MULTIPLY && (x.MinValue < 0 || x.MaxValue < 0))
                    return new ValidationResult(false, "Multiply values cannot be negative");
            }

            return ValidationResult.ValidResult;
        }
    }
}