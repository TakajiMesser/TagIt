namespace TagIt.Shared.Models.Contents
{
    public enum Kinds
    {
        Unknown,
        Folder,
        Document,
        Image,
        Audio,
        Video,
        Bookmark
    }

    public interface IContent
    {
        int ID { get; set; }

        string Name { get; }
        string Path { get; }

        IContentSet Parent { get; set; }

        Kinds Kind { get; }
        long Size { get; }
    }
}
