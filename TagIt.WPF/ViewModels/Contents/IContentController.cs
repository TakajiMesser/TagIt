using System;
using TagIt.Shared.Models.Contents;

namespace TagIt.WPF.ViewModels.Contents
{
    public interface IContentController
    {
        IContent CurrentContent { get; }
        event EventHandler<ContentEventArgs> ContentChanged;

        void OpenContent(IContent content);
        void AddToPlaylist(IContent content);
    }
}
