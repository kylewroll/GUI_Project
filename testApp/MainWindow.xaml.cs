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
using System.IO;

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

        //timer for tracking the song's progress
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            //set time span to seconds
            timer.Interval = TimeSpan.FromSeconds(1);
            //subscrive to OnTimedEvent
            timer.Tick += OnTimedEvent;
        }

        //event that timer.Tick is subscribed to, updates label using system clock, leads to timer updated for paused time after resume button is pressed
        private void OnTimedEvent(object sender, EventArgs e)
        {
            //update progress bar with position of media
            SongProgressBar.Value = musPlayer.Position.TotalSeconds;
            //update clock textbox with position of media
            SongDurationClock.Text = Convert.ToString(musPlayer.Position);
        }
    }
}
