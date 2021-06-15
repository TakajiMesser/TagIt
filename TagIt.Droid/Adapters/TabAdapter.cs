using Android.Runtime;
using AndroidX.Fragment.App;
using System;
using TagIt.Droid.Utilities;

namespace TagIt.Droid.Adapters
{
    public abstract class TabAdapter : FragmentPagerAdapter
    {
        private string[] _tabNames;

        public TabAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public TabAdapter(FragmentManager fragmentManager, string[] tabNames) : base(fragmentManager)
        {
            _tabNames = tabNames;
        }

        public override int Count => _tabNames.Length;

        /*public override Fragment GetItem(int position)
        {
            if (position < 0 || position >= _tabNames.Length) throw new ArgumentOutOfRangeException("Could not handle position " + position);


        }*/

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            if (position < 0 || position >= _tabNames.Length) throw new ArgumentOutOfRangeException("Could not handle position " + position);

            return _tabNames[position].ToJavaString();
        }
    }
}