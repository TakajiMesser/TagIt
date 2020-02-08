namespace TagIt.Shared.Models.Drive
{
    public class DriveVideo : DriveFile
    {
        public DriveVideo(Google.Apis.Drive.v3.Data.File dataFile) : base(dataFile) { }
    }
}
