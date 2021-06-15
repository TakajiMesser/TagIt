using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;
using TagIt.Droid.Helpers;
using TagIt.Shared.Models.Contents;

namespace TagIt.Droid.Adapters
{
    public class TagAdapter : MultiSelectListAdapter<IContent>, IFilterable
    {
        public TagAdapter(Context context, IList<IContent> contents) : base(context, contents) => Filter = CreateSearchFilter(c => c.Name);

        public Filter Filter { get; }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_content, parent, false);
            SetUpItemViewClickEvents(view);

            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ViewHolder;
            var content = GetItemAt(position);

            viewHolder.ItemView.Tag = position;
            viewHolder.ItemView.Selected = IsSelected(position);

            viewHolder.NameLabel.Text = content.Name;
            viewHolder.NameLabel.Typeface = FontHelper.GetTypeface(Context, CustomFonts.RobotoCondensedRegular);

            //viewHolder.PreviewImage.SetImageDrawable(ResourcesCompat.GetDrawable(Resource.Drawable.baseline_login_black_36dp));
        }

        private class ViewHolder : RecyclerView.ViewHolder
        {
            public TextView NameLabel { get; set; }
            public ImageView PreviewImage { get; set; }

            public ViewHolder(View itemView) : base(itemView)
            {
                NameLabel = itemView.FindViewById<TextView>(Resource.Id.name_label);
                PreviewImage = itemView.FindViewById<ImageView>(Resource.Id.preview_image);
            }
        }
    }
}