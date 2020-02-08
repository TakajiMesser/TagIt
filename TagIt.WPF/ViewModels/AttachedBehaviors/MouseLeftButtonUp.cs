using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TagIt.WPF.ViewModels.AttachedBehaviors
{
    public class MouseLeftButtonUp
    {
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseLeftButtonUp), new UIPropertyMetadata(CommandChanged));
        public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(MouseLeftButtonUp), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value) => target.SetValue(CommandProperty, value);

        public static void SetCommandParameter(DependencyObject target, object value) => target.SetValue(CommandParameterProperty, value);

        public static object GetCommandParameter(DependencyObject target) => target.GetValue(CommandParameterProperty);

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is UIElement element)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    element.MouseLeftButtonUp += OnMouseLeftButtonUp;
                }
                else if (e.NewValue == null && e.OldValue != null)
                {
                    element.MouseLeftButtonUp -= OnMouseLeftButtonUp;
                }
            }
        }

        private static void OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var dependencyObject = sender as DependencyObject;

            var command = (ICommand)dependencyObject.GetValue(CommandProperty);
            var commandParameter = dependencyObject.GetValue(CommandParameterProperty);

            command.Execute(commandParameter);
        }
    }
}
