using Android.OS;
using Android.Views;
using TagIt.Droid.Components;
using TagIt.Droid.Helpers;
using TagIt.Droid.Models.Logging;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace TagIt.Droid.Fragments
{
    public class RemoteFragment : Fragment
    {
        public static RemoteFragment Instantiate() => new RemoteFragment();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) => inflater.Inflate(Resource.Layout.fragment_log, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ToolbarHelper.ShowToolbar(Activity, "Debug Log");
            ToolbarHelper.ShowToolbarBackButton(Activity);

            var fileTextView = View.FindViewById<FileTextView>(Resource.Id.fileText);
            fileTextView.SetFile(DebugLog.BaseLog.FilePath);
        }
    }
}
