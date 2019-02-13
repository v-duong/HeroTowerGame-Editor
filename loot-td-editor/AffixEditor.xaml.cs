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
using System.Collections.ObjectModel;
using loot_td;

namespace loot_td_editor
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 

    public partial class AffixEditor : UserControl
    {
        public ObservableCollection<AffixBase> Affixes = new ObservableCollection<AffixBase>();

        private int currentID = 0;

        public string AffixType
        {
            get { return (string)GetValue(AffixTypeProperty); }
            set { SetValue(AffixTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AffixType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AffixTypeProperty =
            DependencyProperty.Register("AffixType", typeof(string), typeof(AffixEditor), new PropertyMetadata("PREFIX"));

        public AffixType affixContext;

        public AffixEditor()
        {
            InitializeComponent();
            AffixesList.ItemsSource = Affixes;
            Enum.TryParse(AffixType, out AffixType aff);
            affixContext = aff;
            currentID = Affixes.Count;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {

            AffixBase temp = new AffixBase
            {
                Id = currentID,
                Name = "UNTITLED NEW",
                AffixType = affixContext
            };
            Affixes.Add(temp);
            AffixesList.Items.Refresh();
            currentID = Affixes.Count;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            Affixes.Remove((AffixBase)AffixesList.SelectedItem);
            AffixesList.Items.Refresh();
        }
    }
}
