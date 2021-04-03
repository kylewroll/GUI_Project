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

        public MainWindow()
        {
            InitializeComponent();
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
            }
        }



        //function to play currently selected song, and display corresponding art
        private void SongList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            //creates new uri based on selected song
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            //conversion based on https://stackoverflow.com/questions/6503424/how-to-programmatically-set-the-image-source
            BitmapImage image = new BitmapImage(new Uri(imagePaths[SongList.SelectedIndex]));
            //assigns image path to image box
            AlbumArt.Source = image;

            //sets songName variable, and updates CurrentSongLabel to display it
            string songName = songTitles[SongList.SelectedIndex];
            CurrentSongLabel.Content = "Currently playing: " + songName;

            //player plays song
            musPlayer.Play();
        }

        //function to play all songs from song list
        //DOESNT WORK CURRENTLY
        private void PlayAllSongsButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < songTitles.Length; i++)
            {
                //ends any previously playing songs
                musPlayer.Close();

                //creates new uri based on selected song
                Uri music = new Uri(songPaths[SongList.SelectedIndex + i]);
                //player opens uri
                musPlayer.Open(music);

                //conversion based on https://stackoverflow.com/questions/6503424/how-to-programmatically-set-the-image-source
                BitmapImage image = new BitmapImage(new Uri(imagePaths[SongList.SelectedIndex + i]));
                //assigns image path to image box
                AlbumArt.Source = image;

                //player plays song
                musPlayer.Play();
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

            //creates new uri based on new selected song index
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            //player plays song
            musPlayer.Play();
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

            //creates new uri based on new selected song index
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            //player plays song
            musPlayer.Play();
        }




        //function for shuffling loaded songs
        private void ShuffleSongsButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Close();

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

                //replace image paths (so images are properly updated)
                string imagePathTemp = imagePaths[i];
                imagePaths[i] = imagePaths[select];
                imagePaths[select] = imagePathTemp;

                //add song titles to listbox in new order
                SongList.Items.Add(songTitles[i]);
            }
        }



        //function to resume playing current song
        private void ResumePlayButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Play();
        }

        //function to pause current song
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Pause();
        }
    }
}
