using System.Windows.Controls;
using TagIt.Shared.Models.Viewers;
using TagIt.WPF.Models.Viewers;

namespace TagIt.WPF.ViewModels.Viewers
{
    public class ImageViewerPanelViewModel : ViewerViewModel<ImageViewer>
    {
        private RelayCommand _imageDragCommand;

        public IImageViewer ImageViewer => _viewer;

        /*public RelayCommand ImageDragCommand => _imageDragCommand ?? (_imageDragCommand = new RelayCommand(
            p =>
            {
                var args = (DragEventArgs)p;

                args.

                if (/args.OriginalSource is TagBranchViewModel && /args.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    var tagName = args.Data.GetData(DataFormats.StringFormat) as string;
                    _tagRearranger.RearrangeTag(Name, tagName, args);
                    _viewer.Image;
                }
            },
            p => true
        ));*/

        public void SetElement(Image imageElement) => _viewer = new ImageViewer(imageElement);
    }
}
