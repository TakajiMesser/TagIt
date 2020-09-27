﻿using System.Windows;
using TagIt.WPF.ViewModels.Settings;
using TagIt.WPF.Views.Factories;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace TagIt.WPF.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, IWindow, IPropertyDisplayer
    {
        /*public EditorSettings Settings
        {
            get => _settings;
            private set
            {
                _settings = value;
                SetDefaultView();
                SetDefaultTool();
            }
        }

        private EditorSettings _settings;*/

        public SettingsWindow()
        {
            InitializeComponent();

            ViewModel.Window = this;
            ViewModel.PropertyDisplayer = this;
        }

        private bool _isFullscreen = false;

        public void ToggleFullscreen()
        {
            if (_isFullscreen)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;

                _isFullscreen = false;
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;

                _isFullscreen = false;
            }
        }

        public void SetPropertyVisibility(string propertyName, Visibility visibility)
        {
            foreach (var property in PropertyGrid.Properties)
            {
                if (property is PropertyItem propertyItem && propertyItem.PropertyName == propertyName)
                {
                    propertyItem.Visibility = visibility;
                }
            }
        }

        /*private void SetDefaultView()
        {
            switch (ViewModel.Settings.DefaultView)
            {
                case ViewTypes.All:
                    View_All.IsSelected = true;
                    break;
                case ViewTypes.Perspective:
                    View_Perspective.IsSelected = true;
                    break;
                case ViewTypes.X:
                    View_X.IsSelected = true;
                    break;
                case ViewTypes.Y:
                    View_Y.IsSelected = true;
                    break;
                case ViewTypes.Z:
                    View_Z.IsSelected = true;
                    break;
            }
        }

        private void SetDefaultTool()
        {
            switch (ViewModel.Settings.DefaultTool)
            {
                case SpiceEngine.Game.Tools.Brush:
                    Tool_Brush.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Mesh:
                    Tool_Mesh.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Selector:
                    Tool_Selector.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Texture:
                    Tool_Texture.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Volume:
                    Tool_Volume.IsSelected = true;
                    break;
            }
        }

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ViewComboBox.SelectedItem as ComboBoxItem;

            switch (selectedItem.Content)
            {
                case "All":
                    ViewModel.Settings.DefaultView = ViewTypes.All;
                    break;
                case "Perspective":
                    ViewModel.Settings.DefaultView = ViewTypes.Perspective;
                    break;
                case "X":
                    ViewModel.Settings.DefaultView = ViewTypes.X;
                    break;
                case "Y":
                    ViewModel.Settings.DefaultView = ViewTypes.Y;
                    break;
                case "Z":
                    ViewModel.Settings.DefaultView = ViewTypes.Z;
                    break;
            }
        }

        private void ToolComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ToolComboBox.SelectedItem as ComboBoxItem;

            switch (selectedItem.Content)
            {
                case "Brush":
                    ViewModel.Settings.DefaultTool = SpiceEngine.Game.Tools.Brush;
                    break;
                case "Mesh":
                    ViewModel.Settings.DefaultTool = SpiceEngine.Game.Tools.Mesh;
                    break;
                case "Selector":
                    ViewModel.Settings.DefaultTool = SpiceEngine.Game.Tools.Selector;
                    break;
                case "Texture":
                    ViewModel.Settings.DefaultTool = SpiceEngine.Game.Tools.Texture;
                    break;
                case "Volume":
                    ViewModel.Settings.DefaultTool = SpiceEngine.Game.Tools.Volume;
                    break;
            }
        }*/
    }
}
