﻿using System.Windows;
using System.Windows.Input;

namespace TagIt.WPF.ViewModels.AttachedBehaviors
{
    public class MouseDrop
    {
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseDrop), new UIPropertyMetadata(CommandChanged));
        public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(MouseDrop), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value) => target.SetValue(CommandProperty, value);

        public static void SetCommandParameter(DependencyObject target, object value) => target.SetValue(CommandParameterProperty, value);

        public static object GetCommandParameter(DependencyObject target) => target.GetValue(CommandParameterProperty);

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is UIElement uiElement)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    uiElement.AllowDrop = true;
                    uiElement.DragEnter += OnDragEnter;
                    uiElement.Drop += OnDrop;
                }
                else if (e.NewValue == null && e.OldValue != null)
                {
                    uiElement.Drop -= OnDrop;
                    uiElement.DragEnter -= OnDragEnter;
                    uiElement.AllowDrop = false;
                }
            }
        }

        private static void OnDragEnter(object sender, DragEventArgs e)
        {
            var dependencyObject = sender as DependencyObject;
            var command = (ICommand)dependencyObject.GetValue(CommandProperty);

            if (!command.CanExecute(e.Data))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private static void OnDrop(object sender, DragEventArgs e)
        {
            var dependencyObject = sender as DependencyObject;

            var command = (ICommand)dependencyObject.GetValue(CommandProperty);
            //var commandParameter = dependencyObject.GetValue(CommandParameterProperty);

            command.Execute(e);
        }
    }
}
