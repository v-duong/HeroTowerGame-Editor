using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace loot_td_editor
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:loot_td_editor"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:loot_td_editor;assembly=loot_td_editor"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:FilterBox/>
    ///
    /// </summary>
    public class FilterBox : ComboBox
    {
        private string currentSearchString = string.Empty;
        protected TextBox EditableTextBox => GetTemplateChild("PART_EditableTextBox") as TextBox;

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue != null)
            {
                var view = CollectionViewSource.GetDefaultView(newValue);
                view.Filter += BonusFilter;
            }

            if (oldValue != null)
            {
                var view = CollectionViewSource.GetDefaultView(oldValue);
                if (view != null) view.Filter -= BonusFilter;
            }

            base.OnItemsSourceChanged(oldValue, newValue);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            var view = (ListCollectionView)CollectionViewSource.GetDefaultView(ItemsSource);

            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                    break;

                case Key.Tab:
                case Key.Enter:
                    currentSearchString = string.Empty;
                    view.Refresh();
                    break;

                default:
                    if (!IsDropDownOpen)
                        IsDropDownOpen = true;

                    currentSearchString = Text;
                    view.Refresh();
                    Text = currentSearchString;
                    EditableTextBox.CaretIndex = int.MaxValue;
                    break;
            }
            base.OnKeyUp(e);
        }




        public bool BonusFilter(object item)
        {
            if (item == null)
                return false;
            if (currentSearchString.Length == 0)
                return true;
            return item.ToString().ToLower().Contains(currentSearchString.ToLower());
        }
    }
}