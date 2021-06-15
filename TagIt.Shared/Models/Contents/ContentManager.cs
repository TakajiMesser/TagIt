using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagIt.Shared.Models.Fetch;
using TagIt.Shared.Models.Remote;
using TagIt.Shared.Utilities;

namespace TagIt.Shared.Models.Contents
{
    public class ContentManager : IContentProvider
    {
        private Dictionary<Type, IFetcher> _fetcherByContentType = new Dictionary<Type, IFetcher>();
        private List<IContent> _contents = new List<IContent>();

        private Dictionary<string, CacheManifest> _cacheManifestByFetcherName = new Dictionary<string, CacheManifest>();

        private ConcurrentQueue<int> _removedIDs = new ConcurrentQueue<int>();
        private int _nextAvailableID = 1;
        private object _availableIDLock = new object();

        private string _cachePath;

        public ContentManager(string cachePath)
        {
            _cachePath = cachePath;
            Directory.CreateDirectory(_cachePath);

            LoadCacheManifests();
        }

        private void LoadCacheManifests()
        {
            foreach (var filePath in Directory.GetFiles(_cachePath, "*." + CacheManifest.FILE_EXTENSION))
            {
                var cacheManifest = new CacheManifest(filePath);
                cacheManifest.Load();

                var fetcherName = Path.GetFileNameWithoutExtension(filePath);
                _cacheManifestByFetcherName.Add(fetcherName, new CacheManifest(filePath));
            }
        }

        private void Cacher_ContentCached(object sender, CacheEventArgs e)
        {
            var cacheManifest = _cacheManifestByFetcherName[e.Result.FetcherName];
            cacheManifest.CacheInfos.Add(new CacheInfo()
            {
                ContentPath = e.Result.ContentPath,
                FetcherName = e.Result.FetcherName,
                IsSuccess = e.Result.IsSuccess,
                Time = e.Time
            });
        }

        public void AddFetcher<T>(ContentFetcher<T> fetcher) where T : IContent
        {
            _fetcherByContentType.Add(typeof(T), fetcher);
            fetcher.ContentAdded += Fetcher_ContentAdded;

            if (fetcher is ICacher cacher)
            {
                if (!_cacheManifestByFetcherName.ContainsKey(fetcher.Name))
                {
                    var filePath = Path.Combine(_cachePath, fetcher.Name + "." + CacheManifest.FILE_EXTENSION);
                    _cacheManifestByFetcherName.Add(fetcher.Name, new CacheManifest(filePath));
                }

                cacher.ContentCached += Cacher_ContentCached;
            }
        }

        private void Fetcher_ContentAdded(object sender, ContentEventArgs e) => AddContent(e.Content);

        public ContentFetcher<T> GetFetcher<T>() where T : IContent => (ContentFetcher<T>)_fetcherByContentType[typeof(T)];
        public bool HasFetcher<T>() where T : IContent => _fetcherByContentType.ContainsKey(typeof(T));

        public IContent GetContent(int id) => _contents[id];

        public void AddContent(IContent content)
        {
            if (content.ID == 0)
            {
                var id = GetUniqueID();
                content.ID = id;
            }

            if (_contents.Count == content.ID - 1)
            {
                _contents.Add(content);
            }
            else
            {
                _contents[content.ID - 1] = content;
            }
        }

        public void AddContents(IEnumerable<IContent> contents)
        {
            var availableID = 0;
            var index = 0;

            foreach (var content in contents)
            {
                var id = 0;

                if (availableID == 0)
                {
                    if (_removedIDs.TryDequeue(out int result))
                    {
                        id = result;
                        index++;
                    }
                    else
                    {
                        // The moment we fail to dequeue, we should increment nextAvailableID PAST where we need it for the remainder of this loop
                        lock (_availableIDLock)
                        {
                            availableID = _nextAvailableID;
                            _nextAvailableID += contents.Count() - index;

                            // Subtract 2 because we have already incremented the available ID ahead of what we have, AND because the first ID starts at 1
                            _contents.PadTo(null, _nextAvailableID - 2);
                        }
                    }
                }

                // Recheck if we set a valid available ID
                if (availableID > 0)
                {
                    id = availableID;
                    availableID++;
                }

                content.ID = id;
                _contents[content.ID - 1] = content;
            }
        }

        private int GetUniqueID()
        {
            if (_removedIDs.TryDequeue(out int result))
            {
                return result;
            }
            else
            {
                lock (_availableIDLock)
                {
                    var id = _nextAvailableID;
                    _nextAvailableID++;
                    return id;
                }
            }
        }
    }
}
