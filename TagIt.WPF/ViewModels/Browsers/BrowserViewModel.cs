using System.Windows.Controls;
using TagIt.WPF.Models.Videos;

namespace TagIt.WPF.ViewModels.Browsers
{
    public class BrowserViewModel : ViewModel, IBrowser
    {
        private WebBrowser _webBrowser;

        /*private RelayCommand _playPauseCommand;

        public RelayCommand PlayPauseCommand => _playPauseCommand ?? (_playPauseCommand = new RelayCommand(
            p =>
            {
                _videoPlayer.PlayOrPause();
            },
            p => true
        ));*/

        //public void OpenVideo(VideoBookmark videoBookmark) => _webBrowser.Navigate(videoBookmark.Url);

        public void SetWebBrowser(WebBrowser webBrowser) => _webBrowser = webBrowser;
    }
}
