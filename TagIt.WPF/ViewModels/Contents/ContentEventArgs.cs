using System;
using TagIt.Shared.Models.Contents;

namespace TagIt.WPF.ViewModels.Contents
{
    public class ContentEventArgs : EventArgs
    {
        public IContent Content { get; }

        public ContentEventArgs(IContent content) => Content = content;
    }
}
