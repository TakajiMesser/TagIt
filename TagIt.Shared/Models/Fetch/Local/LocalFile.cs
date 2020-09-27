using System.IO;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Local
{
    public class LocalFile : Content, ILocalContent
    {
        public LocalFile(string filePath) : base(System.IO.Path.GetFileNameWithoutExtension(filePath), filePath)
        {
            var fileInfo = new FileInfo(Path);

            if (fileInfo.Exists)
            {
                var extension = System.IO.Path.GetExtension(Path);
                Kind = ParseKind(extension);

                Size = fileInfo.Length;

                CreationTime = fileInfo.CreationTime;
                LastWriteTime = fileInfo.LastWriteTime;
                LastAccessTime = fileInfo.LastAccessTime;

                Exists = true;
            }
        }

        public bool Exists { get; private set; }

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
