using System.IO;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Local
{
    public class LocalFolder : ContentSet<ILocalContent>, ILocalContent
    {
        public LocalFolder(string filePath) : base(System.IO.Path.GetFileNameWithoutExtension(filePath), filePath)
        {
            var fileInfo = new FileInfo(Path);

            if (fileInfo.Exists)
            {
                Exists = true;
                Size = fileInfo.Length;
                CreationTime = fileInfo.CreationTime;
                LastWriteTime = fileInfo.LastWriteTime;
                LastAccessTime = fileInfo.LastAccessTime;
            }
        }

        public bool Exists { get; private set; }
    }
}
