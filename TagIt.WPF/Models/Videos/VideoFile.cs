using System;
using System.IO;
using System.Windows.Media.Imaging;
using TagIt.WPF.Models.Explorers;

namespace TagIt.WPF.Models.Videos
{
    /*public class VideoFile : IPathInfo, IVideo
    {
        public VideoFile() { }
        public VideoFile(string filePath)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(filePath);
            Path = filePath;
        }

        public IFolderInfo Parent { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }

        public bool Exists { get; private set; }
        public long FileSize { get; private set; }

        public DateTime? CreationTime { get; private set; }
        public DateTime? LastWriteTime { get; private set; }
        public DateTime? LastAccessTime { get; private set; }

        // TODO - Initialize as default preview icon based on component type. Should load same default icon statically and share across all components to save memory
        private BitmapImage _previewBitmap;
        //public BitmapImage PreviewBitmap { get; set; }
        public BitmapImage PreviewBitmap
        {
            get => _previewBitmap;
            set
            {
                _previewBitmap = value;
                IconUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler IconUpdated;

        public void Refresh()
        {
            var fileInfo = new FileInfo(Path);

            if (fileInfo.Exists)
            {
                Exists = true;
                FileSize = fileInfo.Length;
                CreationTime = fileInfo.CreationTime;
                LastWriteTime = fileInfo.LastWriteTime;
                LastAccessTime = fileInfo.LastAccessTime;
            }
            else
            {
                Exists = false;
                FileSize = 0;
                CreationTime = null;
                LastWriteTime = null;
                LastAccessTime = null;
            }
        }
    }*/
}
