using System.Windows;
using TagIt.WPF.ViewModels.Viewers;
using TagIt.WPF.Views.Factories;

namespace TagIt.WPF.ViewModels
{
    public class WindowViewModel : ViewModel
    {
        private bool _isFullscreen = false;
        private object _rootContent;

        private RelayCommand _fullscreenCommand;
        private RelayCommand _closeCommand;

        public string Title { get; set; }
        public IWindow Window { get; set; }
        public IViewerViewModel ViewerViewModel { get; set; }

        public RelayCommand FullscreenCommand => _fullscreenCommand ?? (_fullscreenCommand = new RelayCommand(
            p => ToggleFullscreen()
            ));

        public RelayCommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(
            p => Window.Close()
            ));

        public void ToggleFullscreen()
        {
            /*if (_isFullscreen)
            {
                Window.WindowStyle = WindowStyle.SingleBorderWindow;
                Window.WindowState = WindowState.Normal;

                Window.Content = _rootContent;
                ViewerViewModel.AttachContentElement();

                _isFullscreen = false;
            }
            else
            {
                Window.WindowStyle = WindowStyle.None;
                Window.WindowState = WindowState.Maximized;

                _rootContent = Window.Content;
                ViewerViewModel.DetachContentElement();
                Window.Content = ViewerViewModel.ContentElement;

                _isFullscreen = true;
            }*/
        }
    }
}
