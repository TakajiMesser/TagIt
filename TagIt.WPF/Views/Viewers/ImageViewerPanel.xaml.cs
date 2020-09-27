using System.Windows.Controls;
using System.Windows.Media;

namespace TagIt.WPF.Views.Viewers
{
    /// <summary>
    /// Interaction logic for ImageViewerPanel.xaml
    /// </summary>
    public partial class ImageViewerPanel : Grid
    {
        private float _zoomValue;

        public ImageViewerPanel()
        {
            InitializeComponent();

            ViewModel.ContentElement = ImageElement;
            ViewModel.SetElement(ImageElement);
        }

        private void ImageElement_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _zoomValue += 0.1f;
            }
            else
            {
                _zoomValue -= 0.1f;
            }

            var scale = new ScaleTransform(_zoomValue, _zoomValue);
            ImageElement.LayoutTransform = scale;
            e.Handled = true;
        }
    }
}
