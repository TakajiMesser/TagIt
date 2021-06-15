using System;
using System.Threading.Tasks;
using TagIt.Shared.Models.Contents;

namespace TagIt.Shared.Models.Fetch
{
    public interface IFetcher
    {
        string Name { get; }

        event EventHandler<ContentEventArgs> ContentAdded;

        Task Fetch();
    }
}
