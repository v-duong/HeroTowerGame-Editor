using loot_td;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for SpawnWeightEntryWindow.xaml
    /// </summary>
    public partial class SpawnWeightEntryWindow : Window
    {
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }

        public ObservableCollection<AffixWeight> dic;
        public AffixWeight editTarget;
        public bool isEdit = false;

        public SpawnWeightEntryWindow()
        {
            InitializeComponent();
            Groups.ItemsSource = GroupTypes;
        }

        public SpawnWeightEntryWindow(AffixWeight w)
        {
            InitializeComponent();
            Groups.ItemsSource = GroupTypes;

            editTarget = w;
            isEdit = true;

            Groups.SelectedItem = w.type;
            WeightInteger.Value = w.weight;
        }

        public GroupType GetSelectedType()
        {
            return (GroupType)Groups.SelectedItem;
        }

        public int GetWeightValue()
        {
            return (int)WeightInteger.Value;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {

            if (Groups.SelectedItem == null)
            {
                MessageBox.Show("Type not selected", "Error", MessageBoxButton.OK);
                return;
            }
            GroupType selectedType = GetSelectedType();
            int t = AffixBase.WeightContainsType(dic, selectedType);
            if (WeightInteger.Value == null)
            {
                MessageBox.Show("Value NaN", "Error", MessageBoxButton.OK);
                return;
            }

            if (t != -1)
            {
                MessageBox.Show("Key Already Exists", "Error", MessageBoxButton.OK);
                return;
            }
            else
            {
                if (isEdit)
                {
                    editTarget.type = selectedType;
                    editTarget.weight = (int)WeightInteger.Value;
                }
                else
                {
                    AffixWeight w = new AffixWeight()
                    {
                        type = selectedType,
                        weight = (int)WeightInteger.Value
                    };
                    dic.Insert(0, w);
                }
                this.Close();
                return;
            }

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
