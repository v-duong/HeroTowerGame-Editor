using loot_td;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 

    public partial class AffixEditor : UserControl
    {
        public List<AffixBase> Affixes = new List<AffixBase>();

        private int currentID = 0;



        public string AffixProp
        {
            get { return (string)GetValue(AffixPropProperty); }
            set { SetValue(AffixPropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AffixProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AffixPropProperty =
            DependencyProperty.Register("AffixProp", typeof(string), typeof(AffixEditor));



        public AffixType affixContext;
        private bool isContextSet = false;


        public AffixEditor()
        {
            InitializeComponent();
            AffixesList.ItemsSource = Affixes;
            if (Affixes.Count >= 1)
                currentID = Affixes[Affixes.Count - 1].Id+1;
            else
                currentID = 0;
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
                AffixType = affixContext
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
                Id = currentID
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

        private void Binding_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }
    }
}
