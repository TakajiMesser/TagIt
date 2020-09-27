using System;
using System.Collections.Generic;
using TagIt.Shared.Models.Contents;

namespace TagIt.WPF.Models.Thumbnails
{
    public class ThumbnailManager// : IThumbnailProvider
    {
        /*public const string FILE_EXTENSION = ".thm";
        //public const int CACHE_LIMIT = 100;
        public const string FILE_PREFIX = "ThumbnailLibrary";

        private Dictionary<int, Thumbnail> _thumbnailByID = new Dictionary<int, Thumbnail>();

        private object _saveLock = new object();

        private ThumbnailManager() { }

        private static ThumbnailManager _instance;
        public static ThumbnailManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ThumbnailManager();
                }

                return _instance;
            }
        }

        public event EventHandler<ThumbnailEventArgs> ThumbnailGenerated;

        public IEnumerable<Thumbnail> GetThumbnails() => _thumbnailByID.Values;

        public bool HasThumbnail(IContent content) => _thumbnailByID.ContainsKey(content.ID);

        public Thumbnail GetThumbnail(IContent content) => _thumbnailByID[content.ID];

        /*public async Task<Thumbnail> GetThumbnail(string filePath)
        {
            if (!HasThumbnail(filePath))
            {
                await Task.Run(() => GenerateThumbnail(filePath));
            }

            return _thumbnailBySourcePath[filePath];
        }*/

        /*public Thumbnail CreateThumbnail(IContent content)
        {
            var thumbnail = new Thumbnail(content);
            _thumbnailByID.Add(content.ID, thumbnail);

            return thumbnail;
        }*/

        /*public void GenerateThumbnail(IVideo video)
        {
            var thumbnail = new Thumbnail(video);

            Task.Run(() =>
            {
                if (!File.Exists(thumbnail.ImageFilePath))
                {
                    CmdHelper.Run(FilePathHelper.FFMPEG_PATH, new List<CmdArg>
                    {
                        CmdArg.Named("itsoffset"),
                        CmdArg.Named("1"),
                        CmdArg.Named("y"),
                        new CmdArg("i", "\"" + thumbnail.VideoFilePath + "\""),
                        new CmdArg("vcodec", "mjpeg"),
                        new CmdArg("vframes", ProgramSettings.VideoFrame.ToString()),
                        CmdArg.Named("an"),
                        new CmdArg("f", "rawvideo"),
                        new CmdArg("s", ProgramSettings.ThumbnailWidth + "x" + ProgramSettings.ThumbnailHeight),
                        CmdArg.Valued(thumbnail.ImageFilePath)
                    });
                }

                thumbnail.GenerateImage();
                ThumbnailGenerated?.Invoke(this, new ThumbnailEventArgs(thumbnail));
            });
        }

        public Thumbnail GenerateThumbnail(string filePath)
        {
            var thumbnailPath = Path.Combine(FilePathHelper.TEMPORARY_THUMBNAIL_DIRECTORY, "temp_thumbnail_" + Path.GetFileNameWithoutExtension(filePath).Replace(' ', '_'));

            CmdHelper.Run(FilePathHelper.FFMPEG_PATH, new List<CmdArg>
            {
                CmdArg.Named("itsoffset"),
                CmdArg.Named("1"),
                CmdArg.Named("y"),
                new CmdArg("i", "\"" + filePath + "\""),
                new CmdArg("vcodec", "mjpeg"),
                new CmdArg("vframes", ProgramSettings.VideoFrame.ToString()),
                CmdArg.Named("an"),
                new CmdArg("f", "rawvideo"),
                new CmdArg("s", ProgramSettings.ThumbnailWidth + "x" + ProgramSettings.ThumbnailHeight),
                CmdArg.Valued(thumbnailPath)
            });

            var thumbnail = new Thumbnail()
            {
                SourceFilePath = filePath
            };

            if (File.Exists(thumbnailPath))
            {
                var imageBytes = File.ReadAllBytes(thumbnailPath);
                _rawThumbnails.Enqueue(Tuple.Create(filePath, imageBytes));

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

                    thumbnail.Image = image;
                }

                File.Delete(thumbnailPath);

                _thumbnailBySourcePath.TryAdd(thumbnail.SourceFilePath, thumbnail);
                ThumbnailGenerated?.Invoke(this, new ThumbnailEventArgs(thumbnail));
            }

            return thumbnail;
        }

        public void GenerateThumbnails(string directoryPath)
        {

        }

        public List<Thumbnail> GenerateThumbnails(IEnumerable<string> filePaths)
        {
            var arguments = new List<CmdArg>()
            {
                CmdArg.Named("itsoffset"),
                CmdArg.Named("1"),
                CmdArg.Named("y")
            };

            var thumbnails = new List<Thumbnail>();

            /*foreach (var filePath in filePaths)
            {
                var thumbnailPath = Path.Combine(FilePathHelper.TEMPORARY_THUMBNAIL_DIRECTORY, "temp_thumbnail_" + Path.GetFileNameWithoutExtension(filePath).Replace(' ', '_'));

                arguments.Add(new CmdArg("i", "\"" + filePath + "\""));
                arguments.Add(new CmdArg("vcodec", "mjpeg"));
                arguments.Add(new CmdArg("vframes", ProgramSettings.VideoFrame.ToString()));
                arguments.Add(CmdArg.Named("an"));
                arguments.Add(new CmdArg("f", "rawvideo"));
                arguments.Add(new CmdArg("s", ProgramSettings.ThumbnailWidth + "x" + ProgramSettings.ThumbnailHeight));

                arguments.Add(CmdArg.Valued(thumbnailPath));
            }

            CmdHelper.Run(FilePathHelper.FFMPEG_PATH, arguments);*

            //var thumbnailPath = Path.Combine(FilePathHelper.TEMPORARY_THUMBNAIL_DIRECTORY, "temp_thumbnail_" + Path.GetFileNameWithoutExtension(filePath).Replace(' ', '_'));
            var thumbnailPath2 = Path.Combine(FilePathHelper.TEMPORARY_THUMBNAIL_DIRECTORY, "%~ni.png");

            arguments.Add(new CmdArg("i", "\"%i\""));
            arguments.Add(new CmdArg("vcodec", "mjpeg"));
            arguments.Add(new CmdArg("vframes", ProgramSettings.VideoFrame.ToString()));
            arguments.Add(CmdArg.Named("an"));
            arguments.Add(new CmdArg("f", "rawvideo"));
            arguments.Add(new CmdArg("s", ProgramSettings.ThumbnailWidth + "x" + ProgramSettings.ThumbnailHeight));

            arguments.Add(CmdArg.Valued(thumbnailPath2));

            CmdHelper.RunLoop(FilePathHelper.FFMPEG_PATH, arguments, filePaths);

            foreach (var filePath in filePaths)
            {
                var thumbnail = new Thumbnail()
                {
                    SourceFilePath = filePath
                };

                //var thumbnailPath = Path.Combine(FilePathHelper.TEMPORARY_THUMBNAIL_DIRECTORY, "temp_thumbnail_" + Path.GetFileNameWithoutExtension(filePath).Replace(' ', '_'));
                var thumbnailPath = Path.Combine(FilePathHelper.TEMPORARY_THUMBNAIL_DIRECTORY, Path.GetFileNameWithoutExtension(filePath) + ".png");

                if (File.Exists(thumbnailPath))
                {
                    var imageBytes = File.ReadAllBytes(thumbnailPath);

                    if (imageBytes.Length > 0)
                    {
                        _rawThumbnails.Enqueue(Tuple.Create(filePath, imageBytes));

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

                            thumbnail.Image = image;
                        }

                        _thumbnailBySourcePath.TryAdd(thumbnail.SourceFilePath, thumbnail);
                        ThumbnailGenerated?.Invoke(this, new ThumbnailEventArgs(thumbnail));
                    }

                    File.Delete(thumbnailPath);
                }

                thumbnails.Add(thumbnail);
            }

            return thumbnails;
        }*/
    }
}
