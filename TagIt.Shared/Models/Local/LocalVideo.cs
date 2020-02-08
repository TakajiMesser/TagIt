using TagIt.Shared.Models.Contents.Video;

namespace TagIt.Shared.Models.Local
{
    public class LocalVideo : LocalFile, IVideo
    {
        public LocalVideo(string filePath) : base(filePath)
        {
            
        }
    }
}
