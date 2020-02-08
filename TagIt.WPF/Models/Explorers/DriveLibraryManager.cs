using System.Collections.Generic;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models.Videos;

namespace TagIt.WPF.Models.Explorers
{
    /*public class DriveLibraryManager : ILibraryFactory
    {
        public const string FILE_EXTENSION = "LM";

        private DriveFolder _rootFolder = new DriveFolder();
        private Dictionary<string, DriveFile> _fileByID = new Dictionary<string, DriveFile>();
        private Dictionary<string, DriveFolder> _folderByID = new Dictionary<string, DriveFolder>();

        public IEnumerable<IVideo> Videos => _fileByID.Values;

        public IVideo GetVideo(string id) => _fileByID[id];
        public IFolderInfo GetFolder(string id) => _folderByID[id];

        public IEnumerable<IPathInfo> GetBasePaths()
        {
            yield return _rootFolder;
        }

        private IEnumerable<IPathInfo> GetChildPaths(IFolderInfo folderInfo)
        {
            for (var i = 0; i < folderInfo.Count; i++)
            {
                yield return folderInfo.GetInfoAt(i);
            }
        }

        public void Load()
        {
            //_folderInfoByPath.Add(_rootPath, rootFolderInfo);
            var parentIDsByChildID = new Dictionary<string, List<string>>();

            var files = DriveHelper.GetAllFiles(); //DriveHelper.GetFileList();

            foreach (var file in files)
            {
                if (file.MimeType == "application/vnd.google-apps.folder")
                {
                    var driveFolder = new DriveFolder(file);
                    _folderByID.Add(driveFolder.ID, driveFolder);

                    if (file.Parents != null && file.Parents.Count > 0)
                    {
                        parentIDsByChildID.Add(driveFolder.ID, new List<string>(file.Parents));
                    }
                    else
                    {
                        _rootFolder.AddPathInfo(driveFolder);
                    }
                }
                else
                {
                    var driveFile = new DriveFile(file);
                    _fileByID.Add(driveFile.ID, driveFile);

                    if (file.Parents != null && file.Parents.Count > 0)
                    {
                        parentIDsByChildID.Add(driveFile.ID, new List<string>(file.Parents));
                    }
                    else
                    {
                        _rootFolder.AddPathInfo(driveFile);
                    }
                }
            }

            foreach (var kvp in parentIDsByChildID)
            {
                var childFile = _fileByID.ContainsKey(kvp.Key) ? (IDrivePathInfo)_fileByID[kvp.Key] : _folderByID[kvp.Key];
                var parentIDs = kvp.Value;

                foreach (var parentID in kvp.Value)
                {
                    if (_folderByID.ContainsKey(parentID))
                    {
                        var parentFolder = _folderByID[parentID];
                        parentFolder.AddPathInfo(childFile);
                    }
                    else
                    {
                        _rootFolder.AddPathInfo(childFile);
                    }
                }
            }
        }
    }*/
}
