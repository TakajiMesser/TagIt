using System;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Previews;

namespace TagIt.WPF.ViewModels.Contents
{
    public interface IContentController
    {
        IContent CurrentContent { get; }
        IPreviewProvider PreviewProvider { get; }

        event EventHandler<ContentEventArgs> ContentChanged;

        void OpenContent(IContent content);
        void AddToPlaylist(IContent content);
    }
}
