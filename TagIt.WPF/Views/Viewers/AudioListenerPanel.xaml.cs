 using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TagIt.WPF.Views.Viewers
{
    /// <summary>
    /// Interaction logic for AudioListenerPanel.xaml
    /// </summary>
    public partial class AudioListenerPanel : Grid
    {
        private DispatcherTimer _currentTimer;

        public AudioListenerPanel()
        {
            InitializeComponent();

            ViewModel.ContentElement = Player;
            ViewModel.SetMediaPlayer(Player);

            // TODO - Only run this timer when a video is playing?
            _currentTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _currentTimer.Tick += CurrentTimer_Tick;
            _currentTimer.Start();
        }

        private void CurrentTimer_Tick(object sender, EventArgs e)
        {
            ProgressSlider.Value = ViewModel.CurrentSeconds;
            CurrentTime.Text = ViewModel.CurrentTimeText;
        }
    }
}
