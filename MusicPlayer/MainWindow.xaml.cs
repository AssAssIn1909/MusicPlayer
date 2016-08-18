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
        private bool trackPositionChanging;

        private MusicPlayerManager _manager { get; set; }
        private bool mouseOnTrackBar { get; set; }
        


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

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_manager.PlayerStatus != PlayerStatus.Stop && !mouseOnTrackBar)
            {
                trackPosistion.Value = _manager.Position.TotalMilliseconds;

                if (_manager.Position == _manager.Length)
                    _manager.PlayNext();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _manager.CleanupPlayback();
            base.OnClosed(e);
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.PlayPrevious();
            trackPosistion.Maximum = _manager.Length.TotalMilliseconds;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Play();
            trackPosistion.Maximum = _manager.Length.TotalMilliseconds;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Stop();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.PlayNext();
            trackPosistion.Maximum = _manager.Length.TotalMilliseconds;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _manager.PlaySelectedSong(listView.SelectedIndex);
            trackPosistion.Maximum = _manager.Length.TotalMilliseconds;
        }

        private void volumeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _manager.Volume = (int) volumeBar.Value;
        }

        private void trackPosistion_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_manager.PlayerStatus != PlayerStatus.Stop && mouseOnTrackBar && !trackPositionChanging)
                _manager.Position = TimeSpan.FromMilliseconds(trackPosistion.Value);

        }

        private void trackPosistion_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseOnTrackBar = true;
            trackPositionChanging = true;
        }

        private void trackPosistion_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseOnTrackBar = false;
            trackPositionChanging = false;
            _manager.Position = TimeSpan.FromMilliseconds(trackPosistion.Value);
        }
    }
}
