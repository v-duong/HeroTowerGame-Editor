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

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for SpawnWeightEntryWindow.xaml
    /// </summary>
    public partial class SpawnWeightEntryWindow : Window
    {
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }

        private GroupType initialType;
        public Dictionary<GroupType, int> dic;
        public bool isEdit = false;

        public SpawnWeightEntryWindow()
        {
            InitializeComponent();
            Groups.ItemsSource = GroupTypes;
        }

        public SpawnWeightEntryWindow(GroupType t, int i)
        {
            initialType = t;
            InitializeComponent();
            Groups.ItemsSource = GroupTypes;

            Groups.SelectedItem = t;
            WeightInteger.Value = i;
            isEdit = true;
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
            GroupType selectedType = GetSelectedType();

            if( WeightInteger.Value == null )
            {
                MessageBox.Show("Value NaN", "Error", MessageBoxButton.OK);
                return;
            }

            if (dic.ContainsKey(selectedType))
            {
                if (isEdit && initialType == selectedType)
                {
                    dic[initialType] = (int)WeightInteger.Value;
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Key Already Exists", "Error", MessageBoxButton.OK);
                    return;
                }
            } else
            {
                if (isEdit)
                    dic.Remove(initialType);
                dic.Add(selectedType, (int)WeightInteger.Value);
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
