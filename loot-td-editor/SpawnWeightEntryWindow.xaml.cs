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
using System.Windows.Shapes;
using System.Diagnostics;
using loot_td;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for SpawnWeightEntryWindow.xaml
    /// </summary>
    public partial class SpawnWeightEntryWindow : Window
    {
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }

        public List<AffixWeight> dic;
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
            if( WeightInteger.Value == null )
            {
                MessageBox.Show("Value NaN", "Error", MessageBoxButton.OK);
                return;
            }

            if (t != -1)
            {
                    MessageBox.Show("Key Already Exists", "Error", MessageBoxButton.OK);
                    return;
            } else
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
                    dic.Insert(0,w);
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
