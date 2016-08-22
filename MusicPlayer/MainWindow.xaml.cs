using MusicPlayer.Enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Check if left button on the track bar is pressed
        private bool trackPositionChanging;
        // Check if pointer is on the track bar
        private bool mouseOnTrackBar { get; set; }
        private MusicPlayerManager _manager { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            mouseOnTrackBar = false;

            _manager = new MusicPlayerManager();

            listView.ItemsSource = _manager.Songs;
            volumeBar.Value = 50;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Track bar position update
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_manager.PlayerStatus != PlayerStatus.Stop)
            {
                secondsText.Text = timeFildTextFormatter();
                if (!mouseOnTrackBar)
                {
                    trackPosistion.Value = _manager.SongPosition.TotalMilliseconds;

                    if ((int)_manager.SongPosition.TotalSeconds == (int)_manager.SongLength.TotalSeconds)
                        _manager.PlayNext();
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _manager.CleanupPlayback();
            base.OnClosed(e);
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            /// Play previous song on the list
            _manager.PlayPrevious();
            trackPosistion.Maximum = _manager.SongLength.TotalMilliseconds;
            changeNowPlayingFild();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            /// Play the song
            _manager.Play();
            trackPosistion.Maximum = _manager.SongLength.TotalMilliseconds;
            changeNowPlayingFild();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            /// Stop playing song
            _manager.Stop();
            changeNowPlayingFild();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            /// Play next song on the list
            _manager.PlayNext();
            trackPosistion.Maximum = _manager.SongLength.TotalMilliseconds;
            changeNowPlayingFild();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /// Play selected song on the list after double click on song
            _manager.PlaySelectedSong(listView.SelectedIndex);
            trackPosistion.Maximum = _manager.SongLength.TotalMilliseconds;
            changeNowPlayingFild();
        }

        private void volumeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /// Change the volume
            _manager.Volume = (int) volumeBar.Value;
        }

        private void trackPosistion_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /// Change track bar position. Update current playing part of song
            if (_manager.PlayerStatus != PlayerStatus.Stop 
                && mouseOnTrackBar
                && !trackPositionChanging)
                _manager.SongPosition = TimeSpan.FromMilliseconds(trackPosistion.Value);

        }

        private void trackPosistion_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /// Update on pressed left button on track position.
            
            trackPositionChanging = true;
        }

        private void trackPosistion_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /// Update after released left button on track position
            
            trackPositionChanging = false;
            _manager.SongPosition = TimeSpan.FromMilliseconds(trackPosistion.Value);
        }

        private void trackPosistion_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseOnTrackBar = true;
        }

        private void trackPosistion_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseOnTrackBar = false;
        }

        private void changeNowPlayingFild()
        {
            nowPlaying.Text = _manager.CurrentPlaySong;
        }

        private string timeFildTextFormatter()
        {
            var currentTime = _manager.SongPosition.TotalSeconds;
            var songTime = _manager.SongLength.TotalSeconds;

            string text = $"{Math.Floor(currentTime / 60)}:{(currentTime % 60).ToString("00")}/{Math.Floor(songTime / 60)}:{(songTime % 60).ToString("00")}";
            return text;
        }
    }
}
