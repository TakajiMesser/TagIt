using System.Windows.Controls;
using TagIt.Shared.Models.Viewers;
using TagIt.WPF.Models.Viewers;

namespace TagIt.WPF.ViewModels.Viewers
{
    public class DocumentReaderPanelViewModel : ViewerViewModel<DocumentReader>
    {
        public IDocumentReader DocumentReader => _viewer;

        public void SetReader(DocumentViewer reader) => _viewer = new DocumentReader(reader);
    }
}
