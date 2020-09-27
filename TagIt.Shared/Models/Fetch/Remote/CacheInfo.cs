using System;

namespace TagIt.Shared.Models.Remote
{
    public class CacheInfo
    {
        public string FetcherName { get; set; }
        public string ContentPath { get; set; }

        public bool IsSuccess { get; set; }
        public DateTime Time { get; set; }
    }
}
