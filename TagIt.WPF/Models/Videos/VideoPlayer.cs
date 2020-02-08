using System;
using System.Windows.Controls;
using TagIt.Shared.Models.Contents.Video;
using TagIt.Shared.Models.Drive;
using TagIt.Shared.Models.Local;

namespace TagIt.WPF.Models.Videos
{
    public class VideoPlayer : IVideoPlayer
    {
        private VideoStates _videoState;

        public VideoPlayer(MediaElement mediaElement)
        {
            MediaElement = mediaElement;
            MediaElement.Loaded += (s, args) => Loaded?.Invoke(this, EventArgs.Empty);
            MediaElement.MediaOpened += (s, args) => Opened?.Invoke(this, EventArgs.Empty);
        }

        public MediaElement MediaElement { get; }
        public IVideo Video { get; private set; }

        public VideoStates VideoState
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

        public event EventHandler Loaded;
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

        public void Load(IVideo video)
        {
            Video = video;

            if (video is LocalVideo videoFile)
            {
                MediaElement.Source = new Uri(videoFile.Path);
            }
            else if (video is DriveVideo driveVideo)
            {
                if (driveVideo.TryDownload())
                {
                    MediaElement.Source = new Uri(driveVideo.LocalPath);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Play()
        {
            VideoState = VideoStates.Playing;
            MediaElement.Play();
        }

        public void Pause()
        {
            VideoState = VideoStates.Paused;
            MediaElement.Pause();
        }

        public void PlayOrPause()
        {
            if (VideoState == VideoStates.Playing)
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
            var isPlaying = VideoState == VideoStates.Playing;

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
            var isPlaying = VideoState == VideoStates.Playing;

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
            VideoState = VideoStates.Stopped;
            MediaElement.Stop();
        }
    }
}
