using System;
using System.Windows.Controls;
using TagIt.Shared.Models.Viewers;
using TagIt.WPF.Models.Viewers;

namespace TagIt.WPF.ViewModels.Viewers
{
    public class AudioListenerPanelViewModel : ViewerViewModel<AudioListener>
    {
        private RelayCommand _playPauseCommand;
        private RelayCommand _stopCommand;
        private RelayCommand _skipForwardCommand;
        private RelayCommand _skipBackwardCommand;

        private RelayCommand _timeDragCommand;
        private RelayCommand _timeDropCommand;

        public IAudioListener AudioListener => _viewer;

        public string TotalTimeText
        {
            get
            {
                if (_viewer != null)
                {
                    var totalTime = _viewer.TotalTime;
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
            get => _viewer != null ? _viewer.CurrentTime.TotalSeconds : 0;
            set
            {
                if (_viewer != null)
                {
                    _viewer.CurrentTime = TimeSpan.FromSeconds(value);
                    InvokePropertyChanged(nameof(CurrentTimeText));
                }
            }
        }

        public string CurrentTimeText => _viewer != null
            ? _viewer.CurrentTime.ToString(@"hh\:mm\:ss")
            : "00:00:00";

        public double Volume
        {
            get => _viewer != null ? _viewer.MediaElement.Volume : 0;
            set
            {
                if (_viewer != null)
                {
                    _viewer.MediaElement.Volume = value;
                    InvokePropertyChanged(nameof(VolumeText));
                }
            }
        }

        public string VolumeText => _viewer != null
            ? Math.Round(_viewer.MediaElement.Volume) + "%"
            : "";

        public double TotalSeconds
        {
            get
            {
                if (_viewer != null)
                {
                    var totalTime = _viewer.TotalTime;
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
            p => _viewer.PlayOrPause(),
            p => _viewer != null
        ));

        public RelayCommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand(
            p => _viewer.Stop(),
            p => _viewer != null
        ));

        public RelayCommand SkipForwardCommand => _skipForwardCommand ?? (_skipForwardCommand = new RelayCommand(
            p => _viewer.SkipForward(),
            p => _viewer != null
        ));

        public RelayCommand SkipBackwardCommand => _skipBackwardCommand ?? (_skipBackwardCommand = new RelayCommand(
            p => _viewer.SkipBackward(),
            p => _viewer != null
        ));

        public RelayCommand TimeDragCommand => _timeDragCommand ?? (_timeDragCommand = new RelayCommand(
            p =>
            {
                _viewer.Pause();
                //var draggedItem = ViewHelper.FindAncestor<>((DependencyObject)p);
                //DragDrop.DoDragDrop(draggedItem, 0, DragDropEffects.Move);
            },
            p => _viewer != null
        ));

        public RelayCommand TimeDropCommand => _timeDropCommand ?? (_timeDropCommand = new RelayCommand(
            p =>
            {
                _viewer.Play();
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
            p => _viewer != null
        ));

        public void SetMediaPlayer(MediaElement mediaElement)
        {
            _viewer = new AudioListener(mediaElement);

            _viewer.ElementLoaded += (s, args) =>
            {
                //_viewer.Play();
                //_viewer.Pause();
            };

            _viewer.Opened += (s, args) =>
            {
                InvokePropertyChanged(nameof(CurrentTimeText));
                InvokePropertyChanged(nameof(TotalTimeText));
                InvokePropertyChanged(nameof(CurrentSeconds));
                InvokePropertyChanged(nameof(TotalSeconds));

                _viewer.Play();
            };

            _viewer.StateChanged += (s, args) =>
            {
                switch (_viewer.PlayerState)
                {
                    case VideoPlayerStates.Playing:
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
