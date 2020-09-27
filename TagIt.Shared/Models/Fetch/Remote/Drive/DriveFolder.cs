using Google.Apis.Drive.v3.Data;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Remote.Drive
{
    public class DriveFolder : ContentSet<IDriveContent>, IDriveContent
    {
        public DriveFolder(File dataFile) : base(dataFile.Name, dataFile.Id)
        {
            Size = dataFile.Size ?? 0L;
            CreationTime = dataFile.CreatedTime;
            LastWriteTime = dataFile.ModifiedTime;
            LastAccessTime = dataFile.ViewedByMeTime;

            if (dataFile.HasThumbnail.HasValue && dataFile.HasThumbnail.Value)
            {
                ThumbnailLink = dataFile.ThumbnailLink;
            }
        }

        public string ThumbnailLink { get; }
    }
}
