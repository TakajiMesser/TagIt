namespace TagIt.WPF.Models.Thumbnails
{
    public class ThumbnailEventArgs
    {
        public Thumbnail Thumbnail { get; }

        public ThumbnailEventArgs(Thumbnail thumbnail) => Thumbnail = thumbnail;
    }
}
