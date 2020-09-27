using System;
using System.Windows.Controls;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Remote;
using TagIt.Shared.Models.Viewers;

namespace TagIt.WPF.Models.Viewers
{
    public class Browser : Viewer, IBrowser
    {
        public Browser(WebBrowser webBrowser) : base(Kinds.Bookmark)
        {
            WebBrowser = webBrowser;
            WebBrowser.Loaded += (s, args) => IsLoaded = true;
        }

        public WebBrowser WebBrowser { get; }

        protected override void OpenContent()
        {
            if (Content is ILocalContent)
            {
                WebBrowser.Source = new Uri(Content.Path);
            }
            else if (Content is IRemoteContent remoteContent)
            {
                /*if (driveContent.TryDownload())
                {
                    MediaElement.Source = new Uri(content.LocalPath);
                }*/
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
