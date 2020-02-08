using System;
using System.Windows.Controls;
using TagIt.Shared.Models.Contents.Video;
using TagIt.WPF.Models.Videos;

namespace TagIt.WPF.ViewModels.Player
{
    public class PlayerViewModel : ViewModel
    {
        private VideoPlayer _videoPlayer;

        private RelayCommand _playPauseCommand;
        private RelayCommand _stopCommand;
        private RelayCommand _skipForwardCommand;
        private RelayCommand _skipBackwardCommand;

        private RelayCommand _timeDragCommand;
        private RelayCommand _timeDropCommand;

        public IVideoPlayer VideoPlayer => _videoPlayer;

        public string TotalTimeText
        {
            get
            {
                if (_videoPlayer != null)
                {
                    var totalTime = _videoPlayer.TotalTime;
                    if (totalTime.HasValue)
                    {
                        return totalTime.Value.ToString(@"hh\:mm\:ss");
                    }
                }

                return "00:00:00";
            }
        }

        public double CurrentSeconds
        {
            get => _videoPlayer != null ? _videoPlayer.CurrentTime.TotalSeconds : 0;
            set
            {
                if (_videoPlayer != null)
                {
                    _videoPlayer.CurrentTime = TimeSpan.FromSeconds(value);
                    InvokePropertyChanged(nameof(CurrentTimeText));
                }
            }
        }

        public string CurrentTimeText => _videoPlayer != null
            ? _videoPlayer.CurrentTime.ToString(@"hh\:mm\:ss")
            : "00:00:00";

        public double Volume
        {
            get => _videoPlayer != null ? _videoPlayer.MediaElement.Volume : 0;
            set
            {
                if (_videoPlayer != null)
                {
                    _videoPlayer.MediaElement.Volume = value;
                    InvokePropertyChanged(nameof(VolumeText));
                }
            }
        }

        public string VolumeText => _videoPlayer != null
            ? Math.Round(_videoPlayer.MediaElement.Volume) + "%"
            : "";

        public double TotalSeconds
        {
            get
            {
                if (_videoPlayer != null)
                {
                    var totalTime = _videoPlayer.TotalTime;
                    if (totalTime.HasValue)
                    {
                        return totalTime.Value.TotalSeconds;
                    }
                }

                return 0;
            }
        }

        public string PlayPauseButtonText { get; set; } = "Play";

        public RelayCommand PlayPauseCommand => _playPauseCommand ?? (_playPauseCommand = new RelayCommand(
            p =>
            {
                _videoPlayer.PlayOrPause();
            },
            p => true
        ));

        public RelayCommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand(
            p => _videoPlayer.Stop(),
            p => true
        ));

        public RelayCommand SkipForwardCommand => _skipForwardCommand ?? (_skipForwardCommand = new RelayCommand(
            p =>
            {
                _videoPlayer.SkipForward();
            },
            p => true
        ));

        public RelayCommand SkipBackwardCommand => _skipBackwardCommand ?? (_skipBackwardCommand = new RelayCommand(
            p => _videoPlayer.SkipBackward(),
            p => true
        ));

        public RelayCommand TimeDragCommand => _timeDragCommand ?? (_timeDragCommand = new RelayCommand(
            p =>
            {
                _videoPlayer.Pause();
                //var draggedItem = ViewHelper.FindAncestor<>((DependencyObject)p);
                //DragDrop.DoDragDrop(draggedItem, 0, DragDropEffects.Move);
            },
            p => true
        ));

        public RelayCommand TimeDropCommand => _timeDropCommand ?? (_timeDropCommand = new RelayCommand(
            p =>
            {
                _videoPlayer.Play();
                /*var args = (DragEventArgs)p;

                if (args.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    if (_videoPlayer != null)
                    {
                        _videoPlayer.CurrentTime = TimeSpan.FromSeconds(ProgressSlider.);
                        InvokePropertyChanged(nameof(CurrentTimeText));
                    }                    //var name = args.Data.GetData(DataFormats.StringFormat);
                    /*var originalSource = args.OriginalSource;
                    var source = args.Source;
                    var position = args.GetPosition(null);
                    _rearranger.Rearrange((string)name, args);
                }*/
            },
            p => true
        ));

        public void OpenVideo(IVideo video) => _videoPlayer.Load(video);

        public void SetMediaPlayer(MediaElement mediaElement)
        {
            _videoPlayer = new VideoPlayer(mediaElement);

            _videoPlayer.Loaded += (s, args) =>
            {
                _videoPlayer.Play();
                _videoPlayer.Pause();
            };

            _videoPlayer.Opened += (s, args) =>
            {
                InvokePropertyChanged(nameof(CurrentTimeText));
                InvokePropertyChanged(nameof(TotalTimeText));
                InvokePropertyChanged(nameof(CurrentSeconds));
                InvokePropertyChanged(nameof(TotalSeconds));
            };

            _videoPlayer.StateChanged += (s, args) =>
            {
                switch (_videoPlayer.VideoState)
                {
                    case VideoStates.Playing:
                        PlayPauseButtonText = "Pause";
                        break;
                    default:
                        PlayPauseButtonText = "Play";
                        break;
                }

                InvokePropertyChanged(nameof(PlayPauseButtonText));
            };
        }

        /*public void SetProgressSlider()
        {

        }*/
    }
}
