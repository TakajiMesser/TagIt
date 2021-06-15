using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagIt.Shared.Helpers;
using TagIt.Shared.Models.Fetch;

namespace TagIt.Shared.Models.Remote.Drive
{
    public class DriveFetcher : ContentFetcher<IDriveContent>, ICacher
    {
        private static string[] _scopes = { DriveService.Scope.DriveReadonly };

        private DriveService _driveService;

        /*public DriveFetcher()
        {
            var credentials = GetCredentials();

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "TagIt.WPF"
            });
        }*/

        public DriveFetcher() : base("Drive") { }

        public event EventHandler<CacheEventArgs> ContentCached;

        private UserCredential GetCredentials()
        {
            using (var stream = new FileStream(FilePathHelper.DRIVE_CREDENTIAL_PATH, FileMode.Open, FileAccess.Read))
            {
                var credentialPath = FilePathHelper.DRIVE_TOKEN_PATH;

                return GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentialPath, true)).Result;
            }
        }

        public override Task Fetch() => Task.Run(() =>
        {
            var credentials = GetCredentials();

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "TagIt.WPF"
            });

            var parentPathsByChildPath = new Dictionary<string, List<string>>();
            var files = GetAllFiles();

            foreach (var dataFile in files)
            {
                if (dataFile.MimeType == "application/vnd.google-apps.folder")
                {
                    var driveFolder = new DriveFolder(dataFile);
                    AddContent(driveFolder);

                    if (dataFile.Parents != null && dataFile.Parents.Count > 0)
                    {
                        parentPathsByChildPath.Add(driveFolder.Path, dataFile.Parents.ToList());
                    }
                    else
                    {
                        _rootPaths.Add(driveFolder.Path);
                    }
                }
                else
                {
                    var driveFile = new DriveFile(dataFile, this);
                    AddContent(driveFile);

                    if (dataFile.Parents != null && dataFile.Parents.Count > 0)
                    {
                        parentPathsByChildPath.Add(driveFile.Path, dataFile.Parents.ToList());
                    }
                    else
                    {
                        _rootPaths.Add(driveFile.Path);
                    }
                }
            }

            foreach (var kvp in parentPathsByChildPath)
            {
                var contentFile = GetContent(kvp.Key);

                foreach (var parentPath in kvp.Value)
                {
                    if (HasContent(parentPath))
                    {
                        if (GetContent(parentPath) is DriveFolder parentFolder && contentFile is DriveFile childFile)
                        {
                            parentFolder.AddContent(childFile);
                        }
                    }
                    else
                    {
                        _rootPaths.Add(kvp.Key);
                    }
                }
            }
        });

        public IDownloadProgress DownloadToFile(string id, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                return _driveService.Files.Get(id).DownloadWithStatus(fileStream);
            }
        }

        public async Task<IDownloadProgress> DownloadToFileAsync(string id, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                return await _driveService.Files.Get(id).DownloadAsync(fileStream);
            }
        }

        public IList<Google.Apis.Drive.v3.Data.File> GetFileList()
        {
            var request = _driveService.Files.List();
            request.Fields = "files(id, name, size, createdTime, modifiedTime, viewedByMeTime, hasThumbnail, thumbnailLink, webContentLink, webViewLink, parents)";

            var response = request.Execute();
            return response.Files;
        }

        public IList<Google.Apis.Drive.v3.Data.File> GetAllFiles()
        {
            var request = _driveService.Files.List();
            request.PageSize = 100;
            request.Fields = "files(id, name, size, createdTime, modifiedTime, viewedByMeTime, hasThumbnail, thumbnailLink, webContentLink, webViewLink, parents, mimeType),nextPageToken";

            var files = new List<Google.Apis.Drive.v3.Data.File>();
            var response = request.Execute();
            files.AddRange(response.Files);

            while (!string.IsNullOrEmpty(response.NextPageToken))
            {
                request.PageToken = response.NextPageToken;
                response = request.Execute();

                files.AddRange(response.Files);
            }

            return files;
        }

        public string GetUri(string id) => _driveService.Files.Get(id).RestPath;

        public async Task<CacheResult> Cache(IRemoteContent content)
        {
            var localPath = Path.Combine(FilePathHelper.CACHE_DIRECTORY, Name + "_" + content.Name.Replace(' ', '_').Replace('!', '_'));
            CacheResult result;

            if (File.Exists(localPath))
            {
                // TODO - Also cache (in a sidecar file?) download time, in case we need to update the cached file
                result = CacheResult.Success(content, Name, localPath);
            }
            else
            {
                var progress = await DownloadToFileAsync(content.Path, localPath);

                if (progress.Status == DownloadStatus.Completed)
                {
                    result = CacheResult.Success(content, Name, localPath);
                }
                else if (progress.Exception != null)
                {
                    result = CacheResult.Failure(content, Name, progress.Exception);
                }
                else
                {
                    result = CacheResult.Failure(content, Name, progress.Status.ToString());
                }
            }

            ContentCached?.Invoke(this, new CacheEventArgs(result, DateTime.Now));
            return result;
        }

        /*private DriveFile CreateDriveFile(Google.Apis.Drive.v3.Data.File dataFile)
        {
            return new DriveVideo(dataFile);
        }*/
    }
}
