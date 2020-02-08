using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TagIt.Shared.Helpers
{
    public static class DriveHelper
    {
        private static string[] _scopes = { DriveService.Scope.DriveReadonly };

        private static DriveService _driveService;

        private static DriveService Service
        {
            get
            {
                if (_driveService == null)
                {
                    var credentials = GetCredentials();

                    _driveService = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credentials,
                        ApplicationName = "TagIt.WPF"
                    });
                }

                return _driveService;
            }
        }


        public static string GetUri(string id) => Service.Files.Get(id).RestPath;

        /*public static void DoOtherShit(string id)
        {
            var credentials = GetCredentials();

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "TagIt.WPF"
            });

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                return await service.Files.Get(id).DownloadAsync(fileStream);
            }
        }*/

        public static IDownloadProgress DownloadToFile(string id, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                return Service.Files.Get(id).DownloadWithStatus(fileStream);
            }
        }

        public static async Task<IDownloadProgress> DownloadToFileAsync(string id, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                return await Service.Files.Get(id).DownloadAsync(fileStream);
            }
        }

        public static IList<Google.Apis.Drive.v3.Data.File> GetFileList()
        {
            var request = Service.Files.List();
            request.Fields = "files(id, name, size, createdTime, modifiedTime, viewedByMeTime, hasThumbnail, thumbnailLink, webContentLink, webViewLink, parents)";

            var response = request.Execute();
            return response.Files;
        }

        public static IList<Google.Apis.Drive.v3.Data.File> GetAllFiles()
        {
            var request = Service.Files.List();
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

        public static UserCredential GetCredentials()
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
    }
}
