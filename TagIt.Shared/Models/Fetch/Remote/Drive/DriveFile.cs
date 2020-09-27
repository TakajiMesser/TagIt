using Google.Apis.Drive.v3.Data;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Remote.Drive
{
    public class DriveFile : RemoteContent, IDriveContent
    {
        public DriveFile(File dataFile, ICacher cacher) : base(dataFile.Name, dataFile.Id, cacher)
        {
            Size = dataFile.Size ?? 0L;
            CreationTime = dataFile.CreatedTime;
            LastWriteTime = dataFile.ModifiedTime;
            LastAccessTime = dataFile.ViewedByMeTime;

            if (dataFile.HasThumbnail.HasValue && dataFile.HasThumbnail.Value)
            {
                ThumbnailLink = dataFile.ThumbnailLink;
            }

            RemotePath = dataFile.WebContentLink;
            //RemotePath = dataFile.WebViewLink;

            // TODO - This is mad janky
            try
            {
                Kind = ParseKind(System.IO.Path.GetExtension(dataFile.Name));
            }
            catch { }
        }

        public string ThumbnailLink { get; }

        private Kinds ParseKind(string extension)
        {
            switch (extension.ToLower())
            {
                case ".mp4":
                    return Kinds.Video;
                case ".mp3":
                    return Kinds.Audio;
                case ".txt":
                case ".pdf":
                case ".doc":
                case ".docx":
                    return Kinds.Document;
                case "png":
                case ".jpg":
                case ".jpeg":
                    return Kinds.Image;
            }

            return Kinds.Unknown;
        }
    }
}
