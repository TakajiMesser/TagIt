using System;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Viewers
{
    public interface IViewer
    {
        Kinds Kind { get; }
        IContent Content { get; }

        event EventHandler ElementLoaded;
        event EventHandler<ContentEventArgs> ContentOpened;

        void Open(IContent content);
    }
}
