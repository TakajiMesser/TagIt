using System;

namespace TagIt.Shared.Models.Remote
{
    public class CacheEventArgs : EventArgs
    {
        public CacheEventArgs(CacheResult result, DateTime time)
        {
            Result = result;
            Time = time;
        }

        public CacheResult Result { get; }
        public DateTime Time { get; }
    }
}
