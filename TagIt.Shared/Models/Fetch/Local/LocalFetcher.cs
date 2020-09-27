using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Fetch;

namespace TagIt.Shared.Models.Local
{
    public class LocalFetcher : ContentFetcher<ILocalContent>
    {
        private string _rootPath;

        public LocalFetcher(IContentProvider contentProvider, string rootPath) : base(contentProvider, "Local") => _rootPath = rootPath;

        public override Task Fetch() => Task.Run(() =>
        {
            foreach (var content in SearchForContents(_rootPath))
            {
                _rootPaths.Add(content.Path);
            }
        });

        private IEnumerable<ILocalContent> SearchForContents(string path)
        {
            foreach (var filePath in Directory.GetFiles(path))
            {
                var localFile = new LocalFile(filePath);
                AddContent(localFile);

                yield return localFile;
            }

            foreach (var directoryPath in Directory.GetDirectories(path))
            {
                var localFolder = new LocalFolder(directoryPath);
                AddContent(localFolder);

                foreach (var content in SearchForContents(directoryPath))
                {
                    localFolder.AddChild(content);
                }

                yield return localFolder;
            }
        }
    }
}
