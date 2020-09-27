using System;
using System.Threading.Tasks;

namespace TagIt.Shared.Models.Remote
{
    public interface ICacher
    {
        event EventHandler<CacheEventArgs> ContentCached;

        Task<CacheResult> Cache(IRemoteContent content);
    }
}
