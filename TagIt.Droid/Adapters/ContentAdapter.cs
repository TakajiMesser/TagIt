using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;
using TagIt.Droid.Helpers;
using TagIt.Shared.Models.Contents;

namespace TagIt.Droid.Adapters
{
    public class ContentAdapter : MultiSelectListAdapter<IContent>, IFilterable
    {


        public ContentAdapter(Context context, IList<IContent> contents) : base(context, contents)
        {
            Filter = CreateSearchFilter(c => c.Name);
        }

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

            switch (content.Kind)
            {
                case Kinds.Folder:
                    viewHolder.PreviewImage.SetImageResource(Resource.Drawable.baseline_work_black_36dp);
                    break;
                case Kinds.Document:
                    viewHolder.PreviewImage.SetImageResource(Resource.Drawable.baseline_description_black_36dp);
                    break;
                case Kinds.Image:
                    LoadThumbnail(viewHolder, position);
                    //viewHolder.PreviewImage.SetImageResource(Resource.Drawable.baseline_contact_support_black_36dp);
                    break;
                case Kinds.Video:
                    viewHolder.PreviewImage.SetImageResource(Resource.Drawable.baseline_visibility_black_36dp);
                    break;
                case Kinds.Audio:
                    viewHolder.PreviewImage.SetImageResource(Resource.Drawable.baseline_volume_up_black_36dp);
                    break;
            }

            //viewHolder.PreviewImage.SetImageDrawable(ResourcesCompat.GetDrawable(Resource.Drawable.baseline_login_black_36dp));
        }

        private async void LoadThumbnail(ViewHolder viewHolder, int position)
        {
            var content = GetItemAt(position);

            var bitmap = await BitmapFactory.DecodeFileAsync(content.Path);
            var thumbnail = await ThumbnailUtils.ExtractThumbnailAsync(bitmap, viewHolder.PreviewImage.Width, viewHolder.PreviewImage.Height);

            if (position == (int)viewHolder.ItemView.Tag)
            {
                viewHolder.PreviewImage.SetImageBitmap(thumbnail);
            }
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