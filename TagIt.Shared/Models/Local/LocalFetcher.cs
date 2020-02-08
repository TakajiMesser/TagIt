using System.Collections.Generic;
using System.IO;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Local
{
    public class LocalFetcher : ContentFetcher<ILocalContent>
    {
        private string _rootPath;

        public LocalFetcher(string rootPath) => _rootPath = rootPath;

        public override void Fetch()
        {
            foreach (var content in SearchForContent(_rootPath))
            {
                _rootIDs.Add(content.Path);
            }
        }

        private IEnumerable<ILocalContent> SearchForContent(string path)
        {
            foreach (var filePath in Directory.GetFiles(path))
            {
                var localFile = CreateLocalContent(filePath);
                _contentByID.Add(filePath, localFile);
                yield return localFile;
            }

            foreach (var directoryPath in Directory.GetDirectories(path))
            {
                var localFolder = new LocalFolder(directoryPath);
                _contentByID.Add(directoryPath, localFolder);

                foreach (var content in SearchForContent(directoryPath))
                {
                    localFolder.AddContent(content);
                }

                yield return localFolder;
            }
        }

        private LocalFile CreateLocalContent(string filePath)
        {
            // TODO - Create content based on file type
            return new LocalVideo(filePath);
        }
    }
}
