using System;

namespace TagIt.Shared.Models.Remote
{
    public class CacheResult
    {
        private CacheResult(IRemoteContent content, string fetcherName)
        {
            ContentID = content.ID;
            ContentPath = content.Path;
            FetcherName = fetcherName;
            RemotePath = content.RemotePath;
        }

        public int ContentID { get; private set; }
        public string ContentPath { get; private set; }
        public string FetcherName { get; private set; }

        public string RemotePath { get; private set; }
        public string CachedPath { get; set; }

        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }

        public static CacheResult Success(IRemoteContent content, string fetcherName, string cachedPath) => new CacheResult(content, fetcherName)
        {
            IsSuccess = true,
            CachedPath = cachedPath
        };

        public static CacheResult Failure(IRemoteContent content, string fetcherName, string message) => new CacheResult(content, fetcherName)
        {
            IsSuccess = false,
            Message = message
        };

        public static CacheResult Failure(IRemoteContent content, string fetcherName, Exception exception) => new CacheResult(content, fetcherName)
        {
            IsSuccess = false,
            Message = exception.Message,
            Exception = exception
        };
    }
}
