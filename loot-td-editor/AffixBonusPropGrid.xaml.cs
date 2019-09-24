using loot_td;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for AffixBonusPropGrid.xaml
    /// </summary>
    public partial class AffixBonusPropGrid : UserControl
    {
        public AffixBase SelectedAffix
        {
            get { return (AffixBase)GetValue(SelectedAffixProperty); }
            set { SetValue(SelectedAffixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedAffix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedAffixProperty =
            DependencyProperty.Register("SelectedAffix", typeof(AffixBase), typeof(AffixBonusPropGrid));

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

        public AffixBonusPropGrid()
        {
            InitializeComponent();
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