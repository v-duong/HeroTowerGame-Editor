using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using loot_td;
using Newtonsoft.Json;

namespace loot_td_editor.Editors
{
    /// <summary>
    /// Interaction logic for AbilityEditor.xaml
    /// </summary>
    public partial class AbilityEditor : UserControl
    {

        public ObservableCollection<AbilityBase> Abilities;
        private int currentID;

        public void InitializeList()
        {
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\abilities\\abilities.editor.json";
            Debug.WriteLine("Initialized abilities");
            if (!System.IO.File.Exists(filePath))
            {
                Abilities = new ObservableCollection<AbilityBase>();
                AbilitiesList.ItemsSource = Abilities;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Abilities = JsonConvert.DeserializeObject<ObservableCollection<AbilityBase>>(json);

            foreach (AbilityBase k in Abilities)
            {
                if (k.IdName == null)
                    k.IdName = "";
            }

            AbilitiesList.ItemsSource = Abilities;
            if (Abilities.Count >= 1)
                currentID = Abilities[Abilities.Count - 1].Id + 1;
            else
                currentID = 0;
        }

        public AbilityEditor()
        {
            InitializeComponent();
            InitializeList();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AbilityBase temp = new AbilityBase
            {
                Id = currentID,
                Name = "UNTITLED NEW",
            };
            Abilities.Add(temp);
            //AbilitiesList.Items.Refresh();
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (AbilitiesList.SelectedItem == null)
                return;
            AbilityBase temp = Helpers.DeepClone((AbilityBase)AbilitiesList.SelectedItem);
            temp.Id = currentID;
            Abilities.Add(temp);
            //AbilitiesList.Items.Refresh();
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (AbilitiesList.SelectedItem == null)
                return;
            Abilities.Remove((AbilityBase)AbilitiesList.SelectedItem);
            //ArchetypesList.Items.Refresh();
        }
    }
}
