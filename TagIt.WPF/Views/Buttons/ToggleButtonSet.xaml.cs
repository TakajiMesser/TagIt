using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TagIt.WPF.Views.Buttons
{
    public partial class ToggleButtonSet : UserControl
    {
        public readonly static DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ToggleButtonSet),
            new PropertyMetadata(Orientation.Horizontal));

        public readonly static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(ToggleButtonSet),
            new PropertyMetadata(null));

        /*public readonly static DependencyProperty ItemSetProperty = DependencyProperty.Register("ItemSet", typeof(IEnumerable<string>), typeof(ToggleButtonSet),
            new PropertyMetadata(ItemSetChanged_Callback));*/

        public ToggleButtonSet()
        {
            InitializeComponent();
        }

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private void MainList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                ViewModel.UpdateSelection(listBox.SelectedItems.Cast<string>());
            }
        }

        /*public IEnumerable<string> ItemSet
        {
            get => (IEnumerable<string>)GetValue(ItemSetProperty);
            set
            {
                SetValue(ItemSetProperty, value);
                UpdateItemsSource(value);
            }
        }

        private static void ItemSetChanged_Callback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is ToggleButtonSet owner && args.NewValue is IEnumerable<string> itemSet)
            {
                owner.UpdateItemsSource(itemSet);
            }
        }

        private void UpdateItemsSource(IEnumerable<string> itemSet)
        {
            var values = itemSet.ToArray();
            MainList.ItemsSource = values;
        }*/
    }
}
