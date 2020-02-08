using System.Collections.Generic;
using System.IO;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;

namespace TagIt.Shared.Models.Playlists
{
    public class Playlist : IPlaylist
    {
        public const string FILE_EXTENSION = ".ply";

        public string Name { get; set; }
        public string FilePath { get; set; }

        public List<IContent> Contents { get; } = new List<IContent>();

        public void Save() => File.WriteAllLines(FilePath, GetLines());

        private IEnumerable<string> GetLines()
        {
            foreach (var content in Contents)
            {
                if (content is LocalVideo videoFile)
                {
                    yield return "[FILE] - " + videoFile.Path;
                }
            }
        }

        public static Playlist Load(string filePath)
        {
            var playlist = new Playlist()
            {
                FilePath = filePath
            };

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("[FILE] - "))
                {
                    var videoPath = line.Substring(9);
                    playlist.Contents.Add(new LocalVideo(videoPath));
                }
            }

            return playlist;
        }
    }
}
