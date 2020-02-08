using System.Collections.Generic;
using System.IO;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models.Videos;

namespace TagIt.WPF.Models.Explorers
{
    /*public class FileLibraryManager : ILibraryFactory
    {
        public const string FILE_EXTENSION = "LM";

        private Dictionary<string, VideoFile> _videoFileByPath = new Dictionary<string, VideoFile>();
        private Dictionary<string, FolderInfo> _folderInfoByPath = new Dictionary<string, FolderInfo>();

        private string _rootPath;

        public FileLibraryManager(string path) => _rootPath = path;

        public IEnumerable<IVideo> Videos => _videoFileByPath.Values;
        //public IEnumerable<IPathInfo> GetVideoPaths() => GetChildPaths(_baseFolderInfo);

        public IVideo GetVideo(string path) => _videoFileByPath[path];
        public IFolderInfo GetFolder(string path) => _folderInfoByPath[path];

        public IEnumerable<IPathInfo> GetBasePaths()
        {
            yield return _folderInfoByPath[_rootPath];
        }

        private IEnumerable<IPathInfo> GetChildPaths(IFolderInfo folderInfo)
        {
            for (var i = 0; i < folderInfo.Count; i++)
            {
                yield return folderInfo.GetInfoAt(i);
            }
        }

        /*public void Save()
        {
            File.WriteAllLines(_rootPath, new[]
            {
                _baseFolderInfo.Path
            });
        }*

        public void Load()
        {
            var rootFolderInfo = new FolderInfo(_rootPath);
            _folderInfoByPath.Add(_rootPath, rootFolderInfo);

            SearchForVideos(_rootPath, rootFolderInfo);
        }

        private void SearchForVideos(string path, FolderInfo folderInfo)
        {
            foreach (var filePath in Directory.GetFiles(path))
            {
                var extension = System.IO.Path.GetExtension(filePath);
                AddVideo(folderInfo, filePath, extension);
            }

            foreach (var directoryPath in Directory.GetDirectories(path))
            {
                var childFolderInfo = new FolderInfo(directoryPath);
                folderInfo.AddPathInfo(childFolderInfo);

                _folderInfoByPath.Add(directoryPath, childFolderInfo);

                SearchForVideos(directoryPath, childFolderInfo);
            }
        }

        private void AddVideo(FolderInfo folderInfo, string filePath, string extension)
        {
            if (PathInfoHelper.IsValidVideoExtension(extension))
            {
                var videoInfo = new VideoFile(filePath);
                _videoFileByPath.Add(filePath, videoInfo);
                folderInfo.AddPathInfo(videoInfo);
            }
        }
    }*/
}
