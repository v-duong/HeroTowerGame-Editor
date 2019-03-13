using System;
using System.Collections.Generic;
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
using Xceed.Wpf.Toolkit;

namespace loot_td_editor.Editors
{
    /// <summary>
    /// Interaction logic for ElementDamage.xaml
    /// </summary>
    public partial class ElementDamage : UserControl
    {
        ElementType element;

        public string ElementProp
        {
            get { return (string)GetValue(ElementPropProperty); }
            set { SetValue(ElementPropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AffixProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementPropProperty =
            DependencyProperty.Register("ElementProp", typeof(string), typeof(ElementDamage), new PropertyMetadata("", new PropertyChangedCallback(OnPropChange)));

        public static void OnPropChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementDamage a = d as ElementDamage;
            a.EleLabel.Content = a.ElementProp;
            Enum.TryParse<ElementType>(a.ElementProp, out a.element);
        }

        public AbilityBase BindBase
        {
            get { return (AbilityBase)GetValue(BindBaseProperty); }
            set { SetValue(BindBaseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindingProper.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindBaseProperty =
            DependencyProperty.Register("BindBase", typeof(AbilityBase), typeof(ElementDamage), new PropertyMetadata(null, new PropertyChangedCallback(OnPropChange2)));

        public static void OnPropChange2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }


        public AbilityDamageBase BindAbilityDamage
        {
            get { return (AbilityDamageBase)GetValue(BindAbilityDamageProperty); }
            set { SetValue(BindAbilityDamageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindAbilityDamage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindAbilityDamageProperty =
            DependencyProperty.Register("BindAbilityDamage", typeof(AbilityDamageBase), typeof(ElementDamage), new PropertyMetadata(null, new PropertyChangedCallback(OnPropChange3)));


        public static void OnPropChange3(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementDamage a = d as ElementDamage;
            
            if (a.BindAbilityDamage != null)
            {
                a.PhysCheck.IsChecked = true;
            } else
                a.PhysCheck.IsChecked = false;

        }



        public ElementDamage()
        {
            InitializeComponent();
            ElementType e;
            Enum.TryParse<ElementType>(ElementProp, out e);

        }

        public void Init() {
            ElementType e;
            Enum.TryParse<ElementType>(ElementProp, out e);

        }
        
        private void Checked(object sender, RoutedEventArgs e)
        {
            if (BindBase == null)
                return;

            if (!BindBase.DamageLevels.ContainsKey(element))
            {
                AbilityDamageBase a = new AbilityDamageBase();

                BindBase.DamageLevels.Add(element, a);
                BindAbilityDamage = BindBase.DamageLevels[element];
                Debug.WriteLine("added new damagebase");
            }
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            if (BindBase == null)
                return;

            if (BindBase.DamageLevels.ContainsKey(element))
            {
                BindBase.DamageLevels.Remove(element);
            }
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void PhysMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (BindBase == null || BindAbilityDamage == null)
            {
                return;
            }

            BindAbilityDamage.CalculateDamage(BindBase.BaseAbilityPower, BindBase.AbilityScaling);
        }
    }
}
