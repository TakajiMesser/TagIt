﻿using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Local
{
    public interface ILocalContent : IContent
    {
        bool Exists { get; }
    }
}
