using System.Windows.Controls;
using TagIt.Shared.Models.Playlists;

namespace TagIt.WPF.Views.Player
{
    public partial class PlaylistView : Grid
    {
        public PlaylistView()
        {
            InitializeComponent();

            ViewModel.Playlist = new Playlist()
            {
                Name = "New Playlist"
            };
        }
    }
}
