using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Drive
{
    public interface IDriveContent : IContent
    {
        string ID { get; }
        //string LocalPath { get; }
        string ThumbnailLink { get; }

        bool TryDownload();
    }
}
