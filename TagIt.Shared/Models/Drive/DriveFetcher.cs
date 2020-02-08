using System.Collections.Generic;
using TagIt.Shared.Helpers;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Drive
{
    public class DriveFetcher : ContentFetcher<IDriveContent>
    {
        public override void Fetch()
        {
            var parentIDsByChildID = new Dictionary<string, List<string>>();
            var files = DriveHelper.GetAllFiles();

            foreach (var dataFile in files)
            {
                if (dataFile.MimeType == "application/vnd.google-apps.folder")
                {
                    var driveFolder = new DriveFolder(dataFile);
                    _contentByID.Add(driveFolder.ID, driveFolder);

                    if (dataFile.Parents != null && dataFile.Parents.Count > 0)
                    {
                        parentIDsByChildID.Add(driveFolder.ID, new List<string>(dataFile.Parents));
                    }
                    else
                    {
                        _rootIDs.Add(driveFolder.ID);
                    }
                }
                else
                {
                    var driveFile = CreateDriveFile(dataFile);
                    _contentByID.Add(driveFile.ID, driveFile);

                    if (dataFile.Parents != null && dataFile.Parents.Count > 0)
                    {
                        parentIDsByChildID.Add(driveFile.ID, new List<string>(dataFile.Parents));
                    }
                    else
                    {
                        _rootIDs.Add(driveFile.ID);
                    }
                }
            }

            foreach (var kvp in parentIDsByChildID)
            {
                var contentFile = _contentByID[kvp.Key];
                var parentIDs = kvp.Value;

                foreach (var parentID in kvp.Value)
                {
                    if (_contentByID.ContainsKey(parentID))
                    {
                        if (_contentByID[parentID] is DriveFolder parentFolder && contentFile is DriveFile childFile)
                        {
                            parentFolder.AddContent(childFile);
                        }
                    }
                    else
                    {
                        _rootIDs.Add(kvp.Key);
                    }
                }
            }
        }

        private DriveFile CreateDriveFile(Google.Apis.Drive.v3.Data.File dataFile)
        {
            return new DriveVideo(dataFile);
        }
    }
}
