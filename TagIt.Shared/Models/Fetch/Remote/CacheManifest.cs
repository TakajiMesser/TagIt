using System;
using System.Collections.Generic;
using TagIt.Shared.Models.Manifests;

namespace TagIt.Shared.Models.Remote
{
    public class CacheManifest : Manifest
    {
        public const string FILE_EXTENSION = "cm";

        public CacheManifest(string filePath) : base(filePath) { }

        public string FetcherName { get; set; }
        public List<CacheInfo> CacheInfos { get; set; }

        protected override IEnumerable<string> WriteLines()
        {
            yield return "<" + FetcherName + ">";

            foreach (var cacheInfo in CacheInfos)
            {
                yield return "[" + cacheInfo.ContentPath + "]";
                yield return "(" + cacheInfo.IsSuccess.ToString() + ")";
                yield return "(" + cacheInfo.Time.ToString("") + ")";
                yield return "";
            }
        }

        protected override void ReadLines(IEnumerable<string> lines)
        {
            CacheInfo cacheInfo = null;
            var valueIndex = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("<") && line.EndsWith(">"))
                {
                    FetcherName = line.Substring(1, line.IndexOf(">") - 1);
                }
                else if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    var contentPath = line.Substring(1, line.IndexOf("]") - 1);
                    cacheInfo = new CacheInfo()
                    {
                        ContentPath = contentPath
                    };

                    CacheInfos.Add(cacheInfo);
                    valueIndex = 0;
                }
                else if (cacheInfo != null && line.StartsWith("(") && line.EndsWith(")"))
                {
                    var value = line.Substring(1, line.IndexOf(")") - 1);

                    if (valueIndex == 0)
                    {
                        cacheInfo.IsSuccess = bool.Parse(value);
                    }
                    else if (valueIndex == 1)
                    {
                        cacheInfo.Time = DateTime.ParseExact(value, "", null);
                    }

                    valueIndex++;
                }
            }
        }
    }
}
