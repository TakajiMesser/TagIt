using TagIt.Shared.Models.Contents;

namespace TagIt.WPF.Views.Factories
{
    public interface IPanelFactory
    {
        void CreateDocumentReaderPanel(IContent content);
        void CreateImageViewerPanel(IContent content);
        void CreateAudioListenerPanel(IContent content);
        void CreateVideoPlayerPanel(IContent content);
        void CreateBrowserPanel(IContent content);

        void OpenLocalExplorerPanel();
        void OpenDriveExplorerPanel();
        void OpenTagTreePanel();
        void OpenTagListPanel();
        void OpenPlaylistPanel();
    }
}
