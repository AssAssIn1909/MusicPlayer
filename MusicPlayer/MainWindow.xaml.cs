using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private MusicPlayerManager _manager { get; set; }
        public int aaaaa { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _manager = new MusicPlayerManager();

            listView.ItemsSource = _manager.Songs;
            //dataGrid.ItemsSource = _manager.Songs;

            
        }

        public void Dispose()
        {
            _manager.Dispose();
        }

        protected override void OnClosed(EventArgs e)
        {
            Dispose();
            base.OnClosed(e);
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.PlayPrevious();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Play();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Stop();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.PlayNext();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _manager.Stop();
            _manager.PlaySelectedSong(listView.SelectedIndex);
        }
    }
}
