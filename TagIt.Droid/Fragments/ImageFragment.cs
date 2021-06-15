using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using TagIt.Droid.Helpers;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace TagIt.Droid.Fragments
{
    public class ImageFragment : Fragment//, AbsListView.IMultiChoiceModeListener
    {
        public static ImageFragment Instantiate(string filePath)
        {
            var fragment = new ImageFragment();

            var bundle = new Bundle();
            bundle.PutString("filePath", filePath);
            fragment.Arguments = bundle;

            return fragment;
        }

        private string _filePath;
        private ImageView _imageView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) => inflater.Inflate(Resource.Layout.fragment_image, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ToolbarHelper.ShowToolbar(Activity, "Pet Logger");
            ToolbarHelper.HideToolbarBackButton(Activity);

            _filePath = savedInstanceState.GetString("filePath");
            _imageView = view.FindViewById<ImageView>(Resource.Id.content_image);
            _imageView.SetImageURI(Uri.FromFile(new Java.IO.File(_filePath)));
        }
    }
}
