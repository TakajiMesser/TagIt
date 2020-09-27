using System.Collections.Generic;
using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Remote.Drive;
using TagIt.Shared.Models.Local;

namespace TagIt.Shared.Models.Previews
{
    public class PreviewManager : IPreviewProvider
    {
        private Dictionary<int, IPreview> _previewByID = new Dictionary<int, IPreview>();

        public PreviewManager(IThumbnailGenerator thumbnailGenerator) => ThumbnailGenerator = thumbnailGenerator;

        public IThumbnailGenerator ThumbnailGenerator { get; }

        public bool ShouldCacheDriveThumbnails { get; set; }

        public async Task<IPreview> GetPreview(IContent content)
        {
            if (_previewByID.ContainsKey(content.ID))
            {
                return _previewByID[content.ID];
            }
            else
            {
                var preview = new Preview(content);

                if (content is ILocalContent localContent)
                {
                    preview.Path = await ThumbnailGenerator.GenerateThumbnail(localContent);
                }
                else if (content is IDriveContent driveContent)
                {
                    if (ShouldCacheDriveThumbnails)
                    {
                        preview.Path = await ThumbnailGenerator.CacheThumbnail(driveContent);
                    }
                    else
                    {
                        preview.Path = driveContent.ThumbnailLink;
                    }
                }

                _previewByID.Add(content.ID, preview);
                return preview;
            }
        }
    }
}
