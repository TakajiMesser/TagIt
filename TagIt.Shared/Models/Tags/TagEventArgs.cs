using System;

namespace TagIt.Shared.Models.Tags
{
    public class TagEventArgs : EventArgs
    {
        public ITag Tag { get; }

        public TagEventArgs(ITag tag) => Tag = tag;
    }
}
