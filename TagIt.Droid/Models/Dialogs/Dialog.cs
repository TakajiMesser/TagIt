using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Annotations;
using AndroidX.Core.Content;

namespace TagIt.Droid.Models.Dialogs
{
    public abstract class Dialog
    {
        private Context _context;
        private AlertDialog.Builder _builder;

        protected Dialog(Context context)
        {
            _context = context;
            _builder = new AlertDialog.Builder(context, Resource.Style.AlertsDialogTheme);
        }

        public void SetTitle([StringRes] int title)
        {
            var titleView = CreateCustomTitle(_context);
            _builder.SetCustomTitle(titleView);
        }

        public void SetMessage([StringRes] int message) => _builder.SetMessage(message);
        public void SetMessage(string message) => _builder.SetMessage(message);

        protected abstract TextView CreateCustomTitle(Context context);

        /*protected AlertDialog Create()
        {
            var dialog = new AlertDialog.Builder(Context, Resource.Style.AlertsDialogTheme)
                .SetCancelable(true)
                .SetCustomTitle(messageText)
                .SetNegativeButton("Cancel", (s, args) => { })
                .SetPositiveButton("OK", (s, args) => positiveAction())
                .Create();

            dialog.Window.SetBackgroundDrawable(ContextCompat.GetDrawable(Context, Resource.Drawable.rounded_border_dark));
            dialog.Show();
        }*/
    }
}