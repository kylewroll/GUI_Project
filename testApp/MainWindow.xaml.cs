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
using System.Windows.Forms;
using System.Windows.Threading;
using System.Diagnostics;
using System.Timers;

namespace testApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Design and code for adding/playing music based off of tutorial by Vijay Thapa: https://www.youtube.com/watch?v=wjmU28ukjwA

        //creates music player
        MediaPlayer musPlayer = new MediaPlayer();

        //array for song titles
        string[] songTitles;
        //array for paths to song files in folder
        string[] songPaths;
        //array for paths to image files in folder
        string[] imagePaths;

        //boolean for determining whether or not images are added
        bool images = false;

        DispatcherTimer timer = new DispatcherTimer();
        
        DateTime start;

        public MainWindow()
        {
            InitializeComponent();

            VolumeLabel.Content = musPlayer.Volume;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += OnTimedEvent;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            SongDurationClock.Text = Convert.ToString(DateTime.Now - start);
        }

        /*
        void UpdateProgressBar(object sender, EventArgs e)
        {
            SongProgressBar.Minimum = 0;
            SongProgressBar.Maximum = musPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            SongProgressBar.Value = musPlayer.Position.TotalSeconds;
        }

        private void SongProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SongDurationClock.Text = TimeSpan.FromSeconds(SongProgressBar.Value).ToString(@"hh\:mm\:ss");
        }
        */
    }
}
