using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Remote
{
    public interface IRemoteContent : IContent
    {
        string RemotePath { get; }
        string CachedPath { get; }

        Task<CacheResult> DownloadAsync();
    }
}
