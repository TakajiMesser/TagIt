using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Previews
{
    public interface IPreviewProvider
    {
        IThumbnailGenerator ThumbnailGenerator { get; }
        bool ShouldCacheDriveThumbnails { get; set; }

        Task<IPreview> GetPreview(IContent content);
    }
}
