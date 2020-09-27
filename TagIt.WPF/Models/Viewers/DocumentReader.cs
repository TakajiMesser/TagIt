using System;
using System.Windows.Controls;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Remote;
using TagIt.Shared.Models.Viewers;

namespace TagIt.WPF.Models.Viewers
{
    public class DocumentReader : Viewer, IDocumentReader
    {
        public DocumentReader(DocumentViewer viewer) : base(Kinds.Document)
        {
            Viewer = viewer;
            Viewer.Loaded += (s, args) => IsLoaded = true;
        }

        public DocumentViewer Viewer { get; }

        protected override void OpenContent()
        {
            if (Content is ILocalContent)
            {
                //string fileName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Here an xps document.xps");
                //XpsDocument document = new XpsDocument(fileName, FileAccess.Read);
                //var document = new FixedDocument();
                //Viewer.Document = new Uri(content.Path);
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
