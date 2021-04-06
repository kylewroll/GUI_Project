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
        TimeSpan interval = TimeSpan.FromSeconds(1);
        DateTime start;
        Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            VolumeLabel.Content = musPlayer.Volume;
        }

        //function to add songs to listbox
        private void AddSongsButton_Click(object sender, RoutedEventArgs e)
        {
            //opens file window to add songs
            OpenFileDialog file = new OpenFileDialog();

            //filters selection to only mp3, wma
            file.Filter = file.Filter = "All Supported Audio | *.mp3; *.wma | MP3s | *.mp3 | WMAs | *.wma";
            //allows for selection of multiple songs
            file.Multiselect = true;

            //adds song name and path to their respective arrays
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                songTitles = file.SafeFileNames;
                songPaths = file.FileNames;

                //adds song names to listbox
                for (int i = 0; i < songTitles.Length; i++)
                {
                    SongList.Items.Add(songTitles[i]);
                }
            }
        }

        //function to add art for songs, functions in same way as adding songs
        private void AddSongArtButton_Click(object sender, RoutedEventArgs e)
        {
            //sets source to null, had to do this to remove a test image from the file
            AlbumArt.Source = null;
            //opens file window to add images
            OpenFileDialog file = new OpenFileDialog();

            //filters selection to only jpgs, pngs
            file.Filter = file.Filter = "All Supported Images | *.jpg; *.png | JPGs | *.jpg | PNGs | *.png";
            //allows for selection of multiple images
            file.Multiselect = true;

            //adds image path to imagePaths array
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imagePaths = file.FileNames;
                images = true;
            }
        }



        //function to play currently selected song, and display corresponding art
        public void SongList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            DispatcherTimer timer = new DispatcherTimer();

            timer.Stop();
            timer.Interval = interval;

            timer.Tick += OnTimedEvent;

            start = DateTime.Now;
            
            

            //creates new uri based on selected song
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            if(images == true)
            {
                //conversion based on https://stackoverflow.com/questions/6503424/how-to-programmatically-set-the-image-source
                BitmapImage image = new BitmapImage(new Uri(imagePaths[SongList.SelectedIndex]));
                //assigns image path to image box
                AlbumArt.Source = image;
            }
            

            //sets songName variable, and updates CurrentSongLabel to display it
            string songName = songTitles[SongList.SelectedIndex];
            CurrentSongLabel.Content = "Currently playing: " + songName;

            //player plays song
            musPlayer.Play();
            
            //starts timer
            timer.Start();
            
        }   

        
        private void OnTimedEvent(object sender, EventArgs e)
        {
                timer.Interval = interval;
                stopwatch.Restart();

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

        //function to play all songs from song list, manually plays first song, then loops through rest till end
        private void PlayAllSongsButton_Click(object sender, RoutedEventArgs e)
        {
            //sets index, so song is visibly selected while playing
            SongList.SelectedIndex = 0;

            timer.Stop();
            timer.Interval = interval;
            timer.Tick += OnTimedEvent;
            start = DateTime.Now;

            //creates and opens Uri for first song
            Uri music = new Uri(songPaths[0]);
            musPlayer.Open(music);

            //play uri
            musPlayer.Play();

            timer.Start();

            //when song over, call player_MediaEnded function
            musPlayer.MediaEnded += player_MediaEnded;
        }

        //function that is called when song is done playing, plays next song
        //based on function from answer in thread https://stackoverflow.com/questions/10482987/automatically-play-next-item-in-listbox?rq=1
        void player_MediaEnded(object sender, EventArgs e)
        {
            //makes currentSongIndex variable, used to play next song
            int currentSongIndex = SongList.SelectedIndex;

            currentSongIndex++;

            //loop called until last song is reached
            if(currentSongIndex < SongList.Items.Count)
            {
                //updates index in listbox
                SongList.SelectedIndex = currentSongIndex;

                timer.Stop();
                timer.Interval = interval;
                timer.Tick += OnTimedEvent;
                start = DateTime.Now;

                if (images == true)
                {
                    //updates album image
                    BitmapImage image = new BitmapImage(new Uri(imagePaths[SongList.SelectedIndex]));
                    AlbumArt.Source = image;
                }
                

                //updates current song label
                string songName = songTitles[SongList.SelectedIndex];
                CurrentSongLabel.Content = "Currently playing: " + songName;

                //creates and opens Uri for next song, plays it
                Uri music = new Uri(songPaths[currentSongIndex]);
                musPlayer.Open(music);


                musPlayer.Play();

                timer.Start();
            }

            //at the end of the last song, stop the player
            else
            {
                musPlayer.Close();
            }
        }


        //function to move player back one song
        private void PreviousSongButton_Click(object sender, RoutedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            //if loop for when previous button is pressed on first song
            if (SongList.SelectedIndex == 0)
            {
                //sets selected index to last item
                SongList.SelectedIndex = SongList.Items.Count - 1;
            }

            else
            {
                //sets selected index backwards 1
                SongList.SelectedIndex = SongList.SelectedIndex - 1;
            }

            timer.Stop();
            timer.Interval = interval;
            timer.Tick += OnTimedEvent;
            start = DateTime.Now;

            //creates and opens new uri based on new selected song index
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            musPlayer.Open(music);

            //player plays song
            musPlayer.Play();

            timer.Start();
        }

        //function to move player forward one song
        private void NextSongButton_Click(object sender, RoutedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            //if loop for when next is pressed on the last song
            if(SongList.SelectedIndex == SongList.Items.Count - 1)
            {
                //sets selected index to first item
                SongList.SelectedIndex = 0;
            }
            
            else
            {
                //sets selected index forward 1
                SongList.SelectedIndex = SongList.SelectedIndex + 1;
            }

            timer.Stop();
            timer.Interval = interval;
            timer.Tick += OnTimedEvent;
            start = DateTime.Now;

            //creates and opens new uri based on new selected song index
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            musPlayer.Open(music);

            //player plays song
            musPlayer.Play();

            timer.Start();
        }




        //function for shuffling loaded songs
        private void ShuffleSongsButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Close();
            SongList.SelectedIndex = -1;

            //creates random for shuffling song list
            Random rand = new Random();

            //clears listbox of current songs
            SongList.Items.Clear();

            //loops over titles, generates random number in the scope of the titles, and replaces songs in the order that they are generated
            for (int i = 0; i < songTitles.Length; i++)
            {
                int select = rand.Next(i, songTitles.Length);

                //replace song titles
                string titleTemp = songTitles[i];
                songTitles[i] = songTitles[select];
                songTitles[select] = titleTemp;

                //replace song paths (so player doesnt play song originally at [0])
                string songPathTemp = songPaths[i];
                songPaths[i] = songPaths[select];
                songPaths[select] = songPathTemp;

                if(images == true)
                {
                    //replace image paths (so images are properly updated)
                    string imagePathTemp = imagePaths[i];
                    imagePaths[i] = imagePaths[select];
                    imagePaths[select] = imagePathTemp;
                }
                

                //add song titles to listbox in new order
                SongList.Items.Add(songTitles[i]);
            }
        }



        //function to resume playing current song
        private void ResumePlayButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Play();

            //ToggleTimer();

            timer.Start();
        }

        //function to pause current song
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Pause();

            //ToggleTimer();

            timer.Stop();
        }

        private void ToggleTimer()
        {
            if(timer.IsEnabled)
            {
                timer.IsEnabled = false;
                stopwatch.Restart();
            }

            else
            {
                stopwatch.Stop();
                timer.Interval = interval - stopwatch.Elapsed;
                timer.IsEnabled = true;
            }
        }

        //function to decrease volume by .1, and update volume label, unless volume is at 0
        private void VolumeDownButton_Click(object sender, RoutedEventArgs e)
        {

            if (musPlayer.Volume == 0)
            {
                musPlayer.Volume = 0;
            }

            else
            {
                musPlayer.Volume -= .1;
            }
            
            VolumeLabel.Content = musPlayer.Volume;
        }

        //function to increase volume by .1, and update volume label, unless volume is at 1
        private void VolumeUpButton_Click(object sender, RoutedEventArgs e)
        {
            if(musPlayer.Volume == 1)
            {
                musPlayer.Volume = 1;
            }

            else
            {
                musPlayer.Volume += .1;
            }
            
            VolumeLabel.Content = musPlayer.Volume;
        }
    }
}
