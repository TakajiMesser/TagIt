using System.Collections.Generic;
using System.IO;
using TagIt.Shared.Models.Drive;
using TagIt.Shared.Models.Local;

namespace TagIt.Shared.Models.Tags
{
    public class TagLibrary
    {
        public const string FILE_EXTENSION = ".tags";

        public string FilePath { get; set; }
        public List<Tag> Tags { get; } = new List<Tag>();

        public void Save() => File.WriteAllLines(FilePath, GetLines());

        private IEnumerable<string> GetLines()
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
                    if (content is ILocalContent localContent)
                    {
                        yield return localContent.Path;
                    }
                    else if (content is IDriveContent driveContent)
                    {
                        yield return driveContent.ID;
                    }
                    else
                    {
                        yield return content.Name;
                    }
                }

                yield return "";
            }
        }

        public static TagLibrary Load(string filePath)
        {
            var tagLibrary = new TagLibrary()
            {
                FilePath = filePath
            };

            Tag tag = null;

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    var tagName = line.Substring(1, line.IndexOf("]") - 1);
                    tag = new Tag(tagName);

                    tagLibrary.Tags.Add(tag);
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

            return tagLibrary;
        }
    }
}
