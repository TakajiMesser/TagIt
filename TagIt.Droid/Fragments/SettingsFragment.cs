using Android.OS;
using Android.Views;
using AndroidX.Preference;
using AndroidX.RecyclerView.Widget;
using TagIt.Droid.Components;
//using TagIt.Droid.Components.Preferences;
using TagIt.Droid.Helpers;

namespace TagIt.Droid.Fragments
{
    public class SettingsFragment : PreferenceFragmentCompat
    {
        public static SettingsFragment Instantiate() => new SettingsFragment();

        public static SettingsFragment Instantiate(string screenKey)
        {
            var fragment = new SettingsFragment();

            var bundle = new Bundle();
            bundle.PutString(ArgPreferenceRoot, screenKey);
            fragment.Arguments = bundle;

            return fragment;
        }

        private string _rootKey;

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            _rootKey = rootKey;
            SetPreferencesFromResource(Resource.Xml.preferences, rootKey);
        }

        private void SetUpRecyclerView()
        {
            if (ListView != null && Context != null)
            {
                var layoutManager = new LinearLayoutManager(Context);
                ListView.SetLayoutManager(layoutManager);

                ListView.AddItemDecoration(new HorizontalDividerItemDecoration(Context, layoutManager.Orientation, Resource.Drawable.horizontal_divider_light)
                {
                    ShouldShowBeforeFirst = false,
                    ShouldShowAfterLast = true,
                    PaddingLeft = 12,
                    PaddingRight = 12
                });
            }

            ListView.SetBackgroundColor(Android.Graphics.Color.White);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ToolbarHelper.ShowToolbar(Activity, "Settings");
            ToolbarHelper.ShowToolbarBackButton(Activity);

            SetUpRecyclerView();

            var stayActive = PreferenceManager.FindPreference("stay_active");
            if (stayActive != null)
            {
                stayActive.PreferenceChange += StayActive_PreferenceChange;
            }

            var viewDebugLog = PreferenceManager.FindPreference("view_debug_log");
            if (viewDebugLog != null)
            {
                viewDebugLog.PreferenceClick += ViewDebugLog_PreferenceClick;
            }

            var resetDefaults = PreferenceManager.FindPreference("reset_defaults");
            if (resetDefaults != null)
            {
                resetDefaults.PreferenceClick += ResetDefaults_PreferenceClick;
            }

            var version = PreferenceManager.FindPreference("version");
            if (version != null)
            {
                version.Summary = Activity.PackageManager.GetPackageInfo(Activity.PackageName, 0).VersionName;
            }
        }

        private void StayActive_PreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
        {
            var value = (bool)e.NewValue;

            if (value)
            {
                Activity.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            else
            {
                Activity.Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
            }
        }

        public override void OnNavigateToScreen(PreferenceScreen preferenceScreen)
        {
            base.OnNavigateToScreen(preferenceScreen);

            Activity.SupportFragmentManager.BeginTransaction()
                .AddToBackStack(null)
                .Replace(Resource.Id.content_frame, Instantiate(preferenceScreen.Key))
                .Commit();
        }

        private void ViewDebugLog_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            Activity.SupportFragmentManager.BeginTransaction()
                .AddToBackStack(null)
                .Replace(Resource.Id.content_frame, DebugLogFragment.Instantiate())
                .Commit();
        }

        private void ResetDefaults_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            AlertDialogHelper.DisplayDialog(Activity, "Reset settings to default", "This will reset ALL settings to default values! Are you sure?", () =>
            {
                ProgressDialogHelper.RunTask(Activity, "Resetting settings...", () =>
                {
                    PreferenceHelper.ResetToDefaults();

                    PreferenceScreen = null;
                    SetPreferencesFromResource(Resource.Xml.preferences, _rootKey);
                    //InitializePreferences();
                });
            });
        }
    }
}
