using System.Threading.Tasks;
using TagIt.Shared.Models.Remote.Drive;
using TagIt.Shared.Models.Local;

namespace TagIt.Shared.Models.Previews
{
    public interface IThumbnailGenerator
    {
        Task<string> GenerateThumbnail(ILocalContent localContent);
        Task<string> CacheThumbnail(IDriveContent driveContent);
    }
}
