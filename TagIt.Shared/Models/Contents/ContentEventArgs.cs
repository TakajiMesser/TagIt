using System;

namespace TagIt.Shared.Models.Contents
{
    public class ContentEventArgs : EventArgs
    {
        public IContent Content { get; }

        public ContentEventArgs(IContent content) => Content = content;
    }
}
