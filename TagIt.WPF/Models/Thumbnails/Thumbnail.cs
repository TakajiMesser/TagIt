using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models.Cmd;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Drive;
using TagIt.Shared.Models.Local;

namespace TagIt.WPF.Models.Thumbnails
{
    public class Thumbnail
    {
        private IContent _content;
        private string _imageFilePath;
        private BitmapImage _bitmapImage;

        public Thumbnail(IContent content)
        {
            _content = content;
            _imageFilePath = Path.Combine(FilePathHelper.TEMPORARY_THUMBNAIL_DIRECTORY, "temp_thumbnail_" + _content.Name.Replace(' ', '_'));
        }

        public async Task<BitmapImage> GetImage()
        {
            if (_bitmapImage != null)
            {
                return _bitmapImage;
            }
            else
            {
                await Task.Run(() =>
                {
                    if (!File.Exists(_imageFilePath))
                    {
                        GenerateImageFile();
                    }
                });

                return await Task.Run(() => File.Exists(_imageFilePath) ? ReadImageFromFile() : null);
            }
        }

        private void GenerateImageFile()
        {
            if (_content is LocalVideo localVideo)
            {
                CmdHelper.Run(FilePathHelper.FFMPEG_PATH, new List<CmdArg>
                {
                    CmdArg.Named("itsoffset"),
                    CmdArg.Named("1"),
                    CmdArg.Named("y"),
                    new CmdArg("i", "\"" + localVideo.Path + "\""),
                    new CmdArg("vcodec", "mjpeg"),
                    new CmdArg("vframes", ProgramSettings.VideoFrame.ToString()),
                    CmdArg.Named("an"),
                    new CmdArg("f", "rawvideo"),
                    new CmdArg("s", ProgramSettings.ThumbnailWidth + "x" + ProgramSettings.ThumbnailHeight),
                    CmdArg.Valued(_imageFilePath)
                });
            }
            else if (_content is IDriveContent driveContent)
            {
                if (!string.IsNullOrEmpty(driveContent.ThumbnailLink))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(new Uri(driveContent.ThumbnailLink), _imageFilePath);
                    }
                }
            }
        }

        private BitmapImage ReadImageFromFile()
        {
            var imageBytes = File.ReadAllBytes(_imageFilePath);

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
            else
            {
                return null;
            }
        }
    }
}
