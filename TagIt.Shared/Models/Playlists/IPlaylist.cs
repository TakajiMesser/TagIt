using System.Collections.Generic;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Contents.Video;

namespace TagIt.Shared.Models.Playlists
{
    public interface IPlaylist
    {
        string Name { get; set; }
        string FilePath { get; set; }

        List<IContent> Contents { get; }
    }
}
