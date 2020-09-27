using System.Collections.Generic;
using TagIt.Shared.Models.Manifests;

namespace TagIt.Shared.Models.Tags
{
    public class TagManifest : Manifest
    {
        public const string FILE_EXTENSION = ".tags";

        public TagManifest(string filePath) : base(filePath) { }

        public List<Tag> Tags { get; } = new List<Tag>();

        protected override IEnumerable<string> WriteLines()
        {
            foreach (var tag in Tags)
            {
                yield return "[" + tag.Name + "]";

                if (!string.IsNullOrEmpty(tag.Category))
                {
                    yield return "(" + tag.Category + ")";
                }

                foreach (var content in tag.Contents)
                {
                    yield return content.Path;
                }

                yield return "";
            }
        }

        protected override void ReadLines(IEnumerable<string> lines)
        {
            Tag tag = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    var tagName = line.Substring(1, line.IndexOf("]") - 1);
                    tag = new Tag(tagName);

                    Tags.Add(tag);
                }
                else if (tag != null && line.StartsWith("(") && line.EndsWith(")"))
                {
                    var category = line.Substring(1, line.IndexOf(")") - 1);
                    tag.Category = category;
                }
                else if (tag != null && !string.IsNullOrEmpty(line))
                {
                    // TODO - Get content from appropriate fetcher here!
                    //tag.AddVideo(new VideoFile(line));
                }
            }
        }
    }
}
