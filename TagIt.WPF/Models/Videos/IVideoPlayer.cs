using System;
using TagIt.Shared.Models.Contents.Video;

namespace TagIt.WPF.Models.Videos
{
    public enum VideoStates
    {
        Playing,
        Paused,
        Stopped
    }

    public interface IVideoPlayer
    {
        VideoStates VideoState { get; }
        IVideo Video { get; }

        event EventHandler Loaded;
        event EventHandler Opened;
    }
}
