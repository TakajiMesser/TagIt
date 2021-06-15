using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using TagIt.Droid.Adapters;
using TagIt.Droid.Components;
using TagIt.Droid.Helpers;
using TagIt.Droid.Models.Logging;
using TagIt.Shared.Models.Playlists;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace TagIt.Droid.Fragments
{
    public class PlaylistFragment : Fragment
    {
        public static PlaylistFragment Instantiate(IPlaylist playlist) => new PlaylistFragment();

        private PlaylistAdapter _playlistAdapter;
        private FloatingActionButton _addPlaylistFab;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) => inflater.Inflate(Resource.Layout.fragment_playlists, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ToolbarHelper.ShowToolbar(Activity, "Playlists");
            ToolbarHelper.ShowToolbarBackButton(Activity);

            var playlistRecycler = view.FindViewById<RecyclerView>(Resource.Id.playlist_recycler);
            SetUpPlaylistRecycler(playlistRecycler, view);

            var fileTextView = View.FindViewById<FileTextView>(Resource.Id.fileText);
            fileTextView.SetFile(DebugLog.BaseLog.FilePath);
        }

        private void SetUpPlaylistRecycler(RecyclerView recyclerView, View view)
        {
            ((SimpleItemAnimator)recyclerView.GetItemAnimator()).SupportsChangeAnimations = false;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            recyclerView.AddItemDecoration(new VerticalSpaceItemDecoration(20)
            {
                ShouldShowBeforeFirst = true
            });

            /*_playlistAdapter = new PlaylistAdapter(Activity, GetIncidentLoggers().ToList());
            //_loggerAdapter.SetMultiChoiceModeListener(this);
            _playlistAdapter.ItemClick += (s, args) => FragmentHelper.Push(Activity, IncidentDetailsFragment.Instantiate(args.Item.PetID, args.Item.IncidentTypeID));
            _playlistAdapter.IncidentLogged += LoggerAdapter_IncidentLogged;

            recyclerView.SetAdapter(_playlistAdapter);*/
        }
    }
}
