using TagIt.WPF.Helpers;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TagIt.WPF.Models
{
    public sealed class ThumbnailSettings : SettingsSubset
    {
        public ThumbnailSettings() : base("Thumbnail Settings") { }

        public string InitialVideoDirectory { get; set; }

        public int VideoFrame { get; set; }

        public int ThumbnailWidth { get; set; }
        public int ThumbnailHeight { get; set; }

        public int ThumbnailSaveChunk { get; set; }
        public int ThumbnailByteLimit { get; set; }

        public int ThumbnailLoadBatch { get; set; }

        protected override void LoadDefault()
        {
            InitialVideoDirectory = FilePathHelper.INITIAL_LOCAL_DIRECTORY;
            VideoFrame = 500;
            ThumbnailWidth = 100;
            ThumbnailHeight = 100;
            ThumbnailSaveChunk = 10;
            ThumbnailByteLimit = 1000000; // 1 MB
            ThumbnailLoadBatch = 10;
        }
    }
}
