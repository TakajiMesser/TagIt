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

namespace TagIt.Droid.Models.Dialogs
{
    public class Alert : Dialog
    {
        protected Alert(Context context) : base(context) { }

        protected override TextView CreateCustomTitle(Context context)
        {
            throw new NotImplementedException();
        }

        public static void Show(Context context)
        {
            /*var alert = new Alert(context);
            alert.CreateCustomTitle(context);

            var dialog = alert.Create();
            dialog.Show();*/
        }
    }
}