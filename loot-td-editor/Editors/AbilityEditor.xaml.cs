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
using Xceed.Wpf.Toolkit;

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

        private void Checked(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;

            if (AbilitiesList.SelectedItem == null)
                return;


            AbilityBase selected = (AbilityBase)AbilitiesList.SelectedItem;

            switch (c.Name)
            {
                case "PhysCheck":
                    if (!selected.HasPhysical)
                    {
                        AbilityDamageBase a = new AbilityDamageBase();
                        selected.DamageLevels.Add(ElementType.NONE, a);
                        Binding min = new Binding();
                        min.Source = a.MinMult;
                    }
                    break;
                case "FireCheck":
                    selected.DamageLevels.Add(ElementType.FIRE, new AbilityDamageBase());
                    break;
                case "ColdCheck":
                    selected.DamageLevels.Add(ElementType.COLD, new AbilityDamageBase());
                    break;
                case "LightningCheck":
                    selected.DamageLevels.Add(ElementType.LIGHTNING, new AbilityDamageBase());
                    break;
                case "EarthCheck":
                    selected.DamageLevels.Add(ElementType.EARTH, new AbilityDamageBase());
                    break;
                case "DivineCheck":
                    selected.DamageLevels.Add(ElementType.DIVINE, new AbilityDamageBase());
                    break;
                case "VoidCheck":
                    selected.DamageLevels.Add(ElementType.VOID, new AbilityDamageBase());
                    break;
                default:
                    break;
            }
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;

            if (AbilitiesList.SelectedItem == null)
                return;


            AbilityBase selected = (AbilityBase)AbilitiesList.SelectedItem;

            switch (c.Name)
            {
                case "PhysCheck":
                    selected.DamageLevels.Remove(ElementType.NONE);
                    break;
                case "FireCheck":
                    selected.DamageLevels.Remove(ElementType.FIRE);
                    break;
                case "ColdCheck":
                    selected.DamageLevels.Remove(ElementType.COLD);
                    break;
                case "LightningCheck":
                    selected.DamageLevels.Remove(ElementType.LIGHTNING);
                    break;
                case "EarthCheck":
                    selected.DamageLevels.Remove(ElementType.EARTH);
                    break;
                case "DivineCheck":
                    selected.DamageLevels.Remove(ElementType.DIVINE);
                    break;
                case "VoidCheck":
                    selected.DamageLevels.Remove(ElementType.VOID);
                    break;
                default:
                    break;
            }
        }
    }
}
