using System;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Drive
{
    public class DriveFolder : ContentSet<IDriveContent>, IDriveContent
    {
        public DriveFolder(Google.Apis.Drive.v3.Data.File dataFile) : base(dataFile.Name)
        {
            ID = dataFile.Id;
            FileSize = dataFile.Size ?? 0;

            CreationTime = dataFile.CreatedTime;
            LastWriteTime = dataFile.ModifiedTime;
            LastAccessTime = dataFile.ViewedByMeTime;

            if (dataFile.HasThumbnail.HasValue && dataFile.HasThumbnail.Value)
            {
                ThumbnailLink = dataFile.ThumbnailLink;
            }
        }

        public string ID { get; }
        public long FileSize { get; }

        public DateTime? CreationTime { get; }
        public DateTime? LastWriteTime { get; }
        public DateTime? LastAccessTime { get; }

        public string ThumbnailLink { get; }

        public bool TryDownload()
        {
            return false;
        }
    }
}
