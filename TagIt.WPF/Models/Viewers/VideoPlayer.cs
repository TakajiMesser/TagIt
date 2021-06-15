using System;
using System.Windows.Controls;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Remote;
using TagIt.Shared.Models.Viewers;

namespace TagIt.WPF.Models.Viewers
{
    public class VideoPlayer : Viewer, IVideoPlayer
    {
        private VideoPlayerStates _videoState;

        public VideoPlayer(MediaElement mediaElement) : base(Kinds.Video)
        {
            MediaElement = mediaElement;
            MediaElement.Loaded += (s, args) => IsLoaded = true;
            MediaElement.MediaOpened += (s, args) => Opened?.Invoke(this, EventArgs.Empty);
        }

        public MediaElement MediaElement { get; }

        public VideoPlayerStates PlayerState
        {
            get => _videoState;
            set
            {
                if (_videoState != value)
                {
                    _videoState = value;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler Opened;
        public event EventHandler StateChanged;

        public TimeSpan CurrentTime
        {
            get => MediaElement.Position;
            set => MediaElement.Position = value;
        }

        public TimeSpan? TotalTime
        {
            get
            {
                var duration = MediaElement.NaturalDuration;
                if (duration.HasTimeSpan)
                {
                    return duration.TimeSpan;
                }
                else
                {
                    return null;
                }
            }
        }

        protected override async void OpenContent()
        {
            if (Content is ILocalContent)
            {
                MediaElement.Source = new Uri(Content.Path);
            }
            else if (Content is IRemoteContent remoteContent)
            {
                var result = await remoteContent.DownloadAsync().ConfigureAwait(true);

                if (result.IsSuccess)
                {
                    MediaElement.Source = new Uri(remoteContent.CachedPath);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Play()
        {
            PlayerState = VideoPlayerStates.Playing;
            MediaElement.Play();
        }

        public void Pause()
        {
            PlayerState = VideoPlayerStates.Paused;
            MediaElement.Pause();
        }

        public void PlayOrPause()
        {
            if (PlayerState == VideoPlayerStates.Playing)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        public void SkipForward()
        {
            var isPlaying = PlayerState == VideoPlayerStates.Playing;

            if (isPlaying)
            {
                Pause();
            }

            var skipAmount = TimeSpan.FromSeconds(10);

            MediaElement.Position = MediaElement.Position <= MediaElement.NaturalDuration.TimeSpan - skipAmount
                ? MediaElement.Position + skipAmount
                : MediaElement.NaturalDuration.TimeSpan;

            if (isPlaying)
            {
                Play();
            }
        }

        public void SkipBackward()
        {
            var isPlaying = PlayerState == VideoPlayerStates.Playing;

            if (isPlaying)
            {
                Pause();
            }

            var skipAmount = TimeSpan.FromSeconds(10);

            MediaElement.Position = MediaElement.Position >= skipAmount
                ? MediaElement.Position - skipAmount
                : TimeSpan.Zero;

            if (isPlaying)
            {
                Play();
            }
        }

        public void Stop()
        {
            PlayerState = VideoPlayerStates.Stopped;
            MediaElement.Stop();
        }
    }
}
