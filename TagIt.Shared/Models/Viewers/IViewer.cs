using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Viewers
{
    public interface IViewer
    {
        Kinds Kind { get; }
        IContent Content { get; }

        void Open(IContent content);
    }
}
