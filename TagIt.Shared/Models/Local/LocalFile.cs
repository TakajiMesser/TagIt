﻿using System;
using System.IO;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Local
{
    public class LocalFile : ILocalContent
    {
        public LocalFile(string filePath)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(filePath);
            Path = filePath;
            Refresh();
        }

        public string Name { get; }
        public IContentSet Parent { get; set; }

        public string Path { get; }
        public bool Exists { get; private set; }
        public long FileSize { get; private set; }

        public DateTime? CreationTime { get; private set; }
        public DateTime? LastWriteTime { get; private set; }
        public DateTime? LastAccessTime { get; private set; }

        private void Refresh()
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
    }
}
