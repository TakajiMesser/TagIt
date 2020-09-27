using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Previews
{
    public class Preview : IPreview
    {
        protected IContent _content;

        public Preview(IContent content) => _content = content;

        public string Path { get; set; }
    }
}
