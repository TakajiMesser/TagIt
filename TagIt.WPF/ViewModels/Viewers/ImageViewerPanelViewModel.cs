using System.Windows.Controls;
using TagIt.Shared.Models.Viewers;
using TagIt.WPF.Models.Viewers;

namespace TagIt.WPF.ViewModels.Viewers
{
    public class ImageViewerPanelViewModel : ViewerViewModel<ImageViewer>
    {
        public IImageViewer ImageViewer => _viewer;

        public void SetElement(Image imageElement) => _viewer = new ImageViewer(imageElement);
    }
}
