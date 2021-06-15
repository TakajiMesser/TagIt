using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TagIt.Shared.Utilities;
using TagIt.WPF.Helpers;

namespace TagIt.WPF.Views.Viewers
{
    /// <summary>
    /// Interaction logic for ImageViewerPanel.xaml
    /// </summary>
    public partial class ImageViewerPanel : Grid
    {
        private Point _previousPosition;
        private float _zoomValue = 1.0f;

        public ImageViewerPanel()
        {
            InitializeComponent();

            ViewModel.ContentElement = ImageElement;
            ViewModel.SetElement(ImageElement);

            ViewModel.ImageViewer.ContentOpened += (s, args) =>
            {
                ImageElement.Width = ImageScroll.ViewportWidth;
                ImageElement.Height = ImageScroll.ViewportHeight;
            };
        }

        private void ImageScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _zoomValue += 0.1f;
            }
            else
            {
                _zoomValue -= 0.1f;
            }

            _zoomValue = _zoomValue.Clamp(1.0f, 5.0f);

            var scale = new ScaleTransform(_zoomValue, _zoomValue);
            ImageElement.LayoutTransform = scale;
            e.Handled = true;
        }

        private void ImageScroll_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => _previousPosition = e.GetPosition(null);

        private void ImageScroll_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(null);
            var difference = _previousPosition - position;

            if (e.LeftButton == MouseButtonState.Pressed && DragHelper.IsSignificantDrag(difference))
            {
                _previousPosition = position;

                ImageScroll.ScrollToHorizontalOffset(ImageScroll.HorizontalOffset + difference.X);
                ImageScroll.ScrollToVerticalOffset(ImageScroll.VerticalOffset + difference.Y);
            }
        }
    }
}
