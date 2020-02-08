namespace TagIt.Shared.Models.Contents
{
    public interface IContent
    {
        string Name { get; }
        IContentSet Parent { get; set; }
    }
}
