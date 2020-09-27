using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Remote.Drive
{
    public interface IDriveContent : IContent
    {
        string ThumbnailLink { get; }
    }
}
