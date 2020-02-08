using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Playlists;

namespace TagIt.WPF.ViewModels.Player
{
    public interface IPlaylistController
    {
        IPlaylist Playlist { get; set; }

        void AddContent(IContent content);
    }
}