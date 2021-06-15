using System;

namespace TagIt.Shared.Models.Contents
{
    public abstract class Content : IContent
    {
        public Content(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public int ID { get; set; }

        public string Name { get; }
        public string Path { get; }

        public IContentSet Parent { get; set; }

        public Kinds Kind { get; protected set; }
        public long Size { get; protected set; }

        public DateTime? CreationTime { get; protected set; }
        public DateTime? LastWriteTime { get; protected set; }
        public DateTime? LastAccessTime { get; protected set; }
    }
}
