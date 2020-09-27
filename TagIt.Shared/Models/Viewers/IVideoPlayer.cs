using System;
using TagIt.Shared.Models.Remote.Drive;

namespace TagIt.Shared.Models.Viewers
{
    public enum VideoPlayerStates
    {
        Playing,
        Paused,
        Stopped
    }

    public interface IVideoPlayer : IViewer
    {
        VideoPlayerStates PlayerState { get; }

        event EventHandler Opened;
    }
}
