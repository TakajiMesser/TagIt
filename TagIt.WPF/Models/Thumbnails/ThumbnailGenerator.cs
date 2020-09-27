using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Previews;
using TagIt.Shared.Models.Remote.Drive;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models.Cmd;

namespace TagIt.WPF.Models.Thumbnails
{
    public class ThumbnailGenerator : IThumbnailGenerator
    {
        public async Task<string> GenerateThumbnail(ILocalContent localContent)
        {
            var thumbnailPath = Path.Combine(FilePathHelper.THUMBNAIL_CACHE_DIRECTORY, "cached_" + localContent.Name.Replace(' ', '_'));

            if (!File.Exists(thumbnailPath))
            {
                await CmdHelper.RunAsync(FilePathHelper.FFMPEG_PATH, new List<CmdArg>
                {
                    CmdArg.Named("itsoffset"),
                    CmdArg.Named("1"),
                    CmdArg.Named("y"),
                    new CmdArg("i", "\"" + localContent.Path + "\""),
                    new CmdArg("vcodec", "mjpeg"),
                    new CmdArg("vframes", ProgramSettings.VideoFrame.ToString()),
                    CmdArg.Named("an"),
                    new CmdArg("f", "rawvideo"),
                    new CmdArg("s", ProgramSettings.ThumbnailWidth + "x" + ProgramSettings.ThumbnailHeight),
                    CmdArg.Valued(thumbnailPath)
                });
            }

            return thumbnailPath;
        }

        public async Task<string> CacheThumbnail(IDriveContent driveContent)
        {
            if (string.IsNullOrEmpty(driveContent.ThumbnailLink)) throw new ArgumentException("Drive content has no thumbnail link");

            var thumbnailPath = Path.Combine(FilePathHelper.THUMBNAIL_CACHE_DIRECTORY, "cached_" + driveContent.Name.Replace(' ', '_'));

            if (!File.Exists(thumbnailPath))
            {
                using (var client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(new Uri(driveContent.ThumbnailLink), thumbnailPath);
                }
            }

            return thumbnailPath;
        }

        public BitmapImage GetThumbnailImage(string path)
        {
            if (File.Exists(path))
            {
                var imageBytes = File.ReadAllBytes(path);

                if (imageBytes.Length > 0)
                {
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        var image = new BitmapImage();
                        stream.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = stream;
                        image.EndInit();
                        image.Freeze();

                        return image;
                    }
                }
            }

            return null;
        }
    }
}
