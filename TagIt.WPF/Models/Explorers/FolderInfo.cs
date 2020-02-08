using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace TagIt.WPF.Models.Explorers
{
    /*public class FolderInfo : IPathInfo, IFolderInfo
    {
        private List<IPathInfo> _pathInfos = new List<IPathInfo>();

        public FolderInfo(string filePath)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(filePath);
            //Name = System.IO.Path.GetDirectoryName(filePath);
            Path = filePath;
        }

        public IFolderInfo Parent { get; set; }
        public IEnumerable<IPathInfo> Items => _pathInfos;

        public string Name { get; private set; }
        public string Path { get; private set; }

        public bool Exists { get; private set; }
        public long FileSize { get; private set; }

        public DateTime? CreationTime { get; private set; }
        public DateTime? LastWriteTime { get; private set; }
        public DateTime? LastAccessTime { get; private set; }

        public BitmapImage PreviewBitmap { get; set; }

        public void AddPathInfo(IPathInfo pathInfo)
        {
            pathInfo.Parent = this;
            _pathInfos.Add(pathInfo);
        }

        public int Count => _pathInfos.Count;

        public IPathInfo GetInfoAt(int index) => _pathInfos[index];

        public IPathInfo GetChild(string name) => _pathInfos.FirstOrDefault(p => p.Name == name);

        public void Refresh()
        {
            var directoryInfo = new DirectoryInfo(Path);

            if (directoryInfo.Exists)
            {
                Exists = true;
                FileSize = _pathInfos.Sum(c => c.FileSize);
                CreationTime = directoryInfo.CreationTime;
                LastWriteTime = directoryInfo.LastWriteTime;
                LastAccessTime = directoryInfo.LastAccessTime;
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

        public void Clear() => _pathInfos.Clear();
    }*/
}
