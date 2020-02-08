using System;
using System.Collections.Generic;
using TagIt.WPF.Helpers;

namespace TagIt.WPF.Models
{
    public class ProgramSettings : BaseSettings<ProgramSettings>
    {
        public const string FILE_EXTENSION = ".user";

        private ThumbnailSettings _thumbnailSettings = new ThumbnailSettings();

        private static Lazy<ProgramSettings> _instance = new Lazy<ProgramSettings>(() => LoadOrDefault(FilePathHelper.SETTINGS_PATH));

        public ProgramSettings() { }

        public static ProgramSettings Instance => LoadOrDefault(FilePathHelper.SETTINGS_PATH);

        public static string InitialVideoDirectory => _instance.Value._thumbnailSettings.InitialVideoDirectory;
        public static int VideoFrame => _instance.Value._thumbnailSettings.VideoFrame;
        public static int ThumbnailWidth => _instance.Value._thumbnailSettings.ThumbnailWidth;
        public static int ThumbnailHeight => _instance.Value._thumbnailSettings.ThumbnailHeight;
        public static int ThumbnailSaveChunk => _instance.Value._thumbnailSettings.ThumbnailSaveChunk;
        public static int ThumbnailByteLimit => _instance.Value._thumbnailSettings.ThumbnailByteLimit;
        public static int ThumbnailLoadBatch => _instance.Value._thumbnailSettings.ThumbnailLoadBatch;

        public override void Commit()
        {
            Save(FilePathHelper.SETTINGS_PATH);
            _instance = new Lazy<ProgramSettings>(() => LoadOrDefault(FilePathHelper.SETTINGS_PATH));
        }

        protected override IEnumerable<ISettingsSubset> GetSubsets()
        {
            yield return _thumbnailSettings;
        }
    }
}
