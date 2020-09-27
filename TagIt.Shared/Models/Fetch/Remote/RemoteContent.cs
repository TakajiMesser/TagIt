using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Remote
{
    public abstract class RemoteContent : Content, IRemoteContent
    {
        private ICacher _cacher;

        public RemoteContent(string name, string path, ICacher cacher) : base(name, path) => _cacher = cacher;
        
        public string RemotePath { get; protected set; }
        public string CachedPath { get; private set; }

        public async Task<CacheResult> DownloadAsync()
        {
            var result = await _cacher.Cache(this);

            if (result.IsSuccess)
            {
                CachedPath = result.CachedPath;
            }

            return result;
        }
    }
}
