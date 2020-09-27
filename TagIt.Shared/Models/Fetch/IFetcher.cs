using System.Threading.Tasks;

namespace TagIt.Shared.Models.Fetch
{
    public interface IFetcher
    {
        string Name { get; }

        Task Fetch();
    }
}
