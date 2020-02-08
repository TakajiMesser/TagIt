using System;
using TagIt.Shared.Helpers;
using TagIt.Shared.Models.Contents;
using TagIt.WPF.Helpers;

namespace TagIt.Shared.Models.Drive
{
    public class DriveFile : IDriveContent
    {
        private bool _isDownloaded = false;

        public DriveFile(Google.Apis.Drive.v3.Data.File dataFile)
        {
            ID = dataFile.Id;
            Name = dataFile.Name;
            LocalPath = System.IO.Path.Combine(TagIt.Shared.Helpers.FilePathHelper.TEMPORARY_DRIVE_DIRECTORY, "temp_download_" + Name.Replace(' ', '_'));

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
        public string Name { get; }
        public IContentSet Parent { get; set; }

        public string LocalPath { get; }
        public long FileSize { get; }

        public DateTime? CreationTime { get; }
        public DateTime? LastWriteTime { get; }
        public DateTime? LastAccessTime { get; }

        public string ThumbnailLink { get; }

        public bool TryDownload()
        {
            if (_isDownloaded)
            {
                return true;
            }
            else
            {
                var progress = DriveHelper.DownloadToFile(ID, LocalPath);

                if (progress.Status == Google.Apis.Download.DownloadStatus.Completed)
                {
                    _isDownloaded = true;
                    return true;
                }
                else if (progress.Status == Google.Apis.Download.DownloadStatus.Failed)
                {
                    if (progress.Exception != null)
                    {
                        throw progress.Exception;
                    }
                }

                return false;
            }
        }
    }
}
