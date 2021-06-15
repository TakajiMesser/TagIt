using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using System.Linq;
using TagIt.Droid.Adapters;
using TagIt.Droid.Components;
using TagIt.Droid.Helpers;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace TagIt.Droid.Fragments
{
    public class LocalFragment : Fragment, IHandleBack, AbsListView.IMultiChoiceModeListener
    {
        public static LocalFragment Instantiate() => new LocalFragment();

        private ContentAdapter _contentAdapter;
        private LocalFetcher _localFetcher;
        //private PreviewManager _previewManager;
        private string _currentPath;
        private bool _isAtRoot;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) => inflater.Inflate(Resource.Layout.fragment_local, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ToolbarHelper.ShowToolbar(Activity, "Pet Logger");
            ToolbarHelper.HideToolbarBackButton(Activity);

            var contentRecycler = view.FindViewById<RecyclerView>(Resource.Id.content_recycler);
            SetUpContentRecycler(contentRecycler, view);

            SetUpFloatingActionMenu(view);
        }

        private async void SetUpContentRecycler(RecyclerView recyclerView, View view)
        {
            ((SimpleItemAnimator)recyclerView.GetItemAnimator()).SupportsChangeAnimations = false;
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            recyclerView.AddItemDecoration(new VerticalSpaceItemDecoration(20)
            {
                ShouldShowBeforeFirst = true
            });

            _currentPath = FilePathHelper.INITIAL_LOCAL_DIRECTORY;
            _isAtRoot = true;

            _localFetcher = new LocalFetcher(FilePathHelper.INITIAL_LOCAL_DIRECTORY);

            //var thumbnailGenerator = new ThumbnailGenerator();
            //_previewManager = new PreviewManager(thumbnailGenerator);

            await _localFetcher.Fetch();

            _contentAdapter = new ContentAdapter(Activity, _localFetcher.RootContents.Cast<IContent>().ToList());
            _contentAdapter.ItemClick += (s, args) => OpenContent(args.Item);

            //_contentAdapter.SetMultiChoiceModeListener(this);
            //_contentAdapter.ItemClick += FragmentHelper.Push(Activity, IncidentDetailsFragment.Instantiate(args.Item.PetID, args.Item.IncidentTypeID));

            recyclerView.SetAdapter(_contentAdapter);
        }

        private void SetUpFloatingActionMenu(View view)
        {
            var fam = view.FindViewById<FloatingActionMenu>(Resource.Id.fam_add);
            //fam.Visibility = ViewStates.Visible;

            var fabAddFile = view.FindViewById<FloatingActionButton>(Resource.Id.fab_add_file);
            //fabAddIncidentType.Click += (s, args) => FragmentHelper.Push(Activity, AddEntityFragment<IncidentType>.Instantiate("Incident Type"));

            var fabAddFolder = view.FindViewById<FloatingActionButton>(Resource.Id.fab_add_folder);
            //fabAddLogger.Click += (s, args) => FragmentHelper.Push(Activity, AddEntityFragment<LoggerDefinition>.Instantiate("Incident Logger"));
        }

        private void OpenContent(IContent content)
        {
            _currentPath = content.Path;
            _isAtRoot = false;

            if (content is IContentSet<ILocalContent> contentSet)
            {
                _contentAdapter.ReplaceItems(contentSet.Contents.Cast<IContent>().ToList());
            }
            else
            {
                switch (content.Kind)
                {
                    case Kinds.Image:
                        FragmentHelper.Push(Activity, ImageFragment.Instantiate(content.Path));
                        break;
                }
                //_contentController.OpenContent(Content);
            }
        }

        public bool OnBackPressed()
        {
            if (_isAtRoot)
            {
                return false;
            }
            else
            {
                var currentContent = _localFetcher.GetContent(_currentPath);

                if (currentContent.Parent != null)
                {
                    OpenContent(currentContent.Parent);
                }
                else
                {
                    _currentPath = FilePathHelper.INITIAL_LOCAL_DIRECTORY;
                    _contentAdapter.ReplaceItems(_localFetcher.RootContents.Cast<IContent>().ToList());
                    _isAtRoot = true;
                }

                return true;
            }
        }

        private void DeleteSelectedItems(ActionMode mode)
        {
            /*AlertDialogHelper.DisplayDialog(Activity, "Remove selected loggers", "Are you sure? This will remove ALL selected incident loggers!", () =>
            {
                var dialog = ProgressDialogHelper.Display(Activity, "Removing selected loggers...");

                ProgressDialogHelper.RunTask(Activity, dialog, () =>
                {
                    _loggerAdapter.RemoveSelectedLoggers();
                    Activity.RunOnUiThread(() => mode.Finish());
                });
            });*/
        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked) { }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_delete:
                    DeleteSelectedItems(mode);
                    return true;
                case Resource.Id.action_select_all:
                    //_loggerAdapter.SetAllItemsChecked(true);
                    return true;
                default:
                    return false;
            }
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            mode.MenuInflater.Inflate(Resource.Menu.menu_list, menu);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode) { }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu) => false;
    }
}
