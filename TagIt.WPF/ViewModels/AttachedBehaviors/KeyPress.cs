using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TagIt.WPF.ViewModels.AttachedBehaviors
{
    public class KeyPress
    {
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(KeyPress), new UIPropertyMetadata(CommandChanged));
        public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(KeyPress), new UIPropertyMetadata(null));
        public static DependencyProperty KeyProperty = DependencyProperty.RegisterAttached("Key", typeof(Key), typeof(KeyPress), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value) => target.SetValue(CommandProperty, value);

        public static void SetCommandParameter(DependencyObject target, object value) => target.SetValue(CommandParameterProperty, value);

        public static object GetCommandParameter(DependencyObject target) => target.GetValue(CommandParameterProperty);

        public static void SetKey(DependencyObject target, Key value) => target.SetValue(KeyProperty, value);

        public static Key GetKey(DependencyObject target) => (Key)target.GetValue(KeyProperty);

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is UIElement element)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    element.KeyDown += OnKeyDown;
                }
                else if (e.NewValue == null && e.OldValue != null)
                {
                    element.KeyDown -= OnKeyDown;
                }
            }
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            var element = sender as UIElement;

            if (e.Key == (Key)element.GetValue(KeyProperty))
            {
                var dependencyObject = sender as DependencyObject;

                var command = (ICommand)dependencyObject.GetValue(CommandProperty);
                var commandParameter = dependencyObject.GetValue(CommandParameterProperty);

                command.Execute(commandParameter);
            }
        }
    }
}
