using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Remote;
using TagIt.Shared.Models.Viewers;

namespace TagIt.WPF.Models.Viewers
{
    public class ImageViewer : Viewer, IImageViewer
    {
        public ImageViewer(Image image) : base(Kinds.Image)
        {
            Image = image;
            Image.Loaded += (s, args) => IsLoaded = true;
        }

        public Image Image { get; }

        protected override void OpenContent()
        {
            if (Content is ILocalContent)
            {
                Image.Source = new BitmapImage(new Uri(Content.Path));
            }
            else if (Content is IRemoteContent remoteContent)
            {
                Image.Source = new BitmapImage(new Uri(remoteContent.RemotePath));
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
