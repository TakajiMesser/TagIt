using System;
using System.Windows.Controls;
using System.Windows.Threading;
using TagIt.WPF.Helpers;

namespace TagIt.WPF.Views.Player
{
    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : Grid
    {
        private DispatcherTimer _currentTimer;

        public PlayerView()
        {
            InitializeComponent();

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
