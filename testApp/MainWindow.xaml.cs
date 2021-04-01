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

        private void AddSongsButton_Click(object sender, RoutedEventArgs e)
        {
            //opens file window to add songs
            OpenFileDialog file = new OpenFileDialog();

            //filters selection for only music
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

        //function to play currently selected song, and display corresponding art
        private void SongList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            //creates new uri based on selected song
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            //conversion magic based on https://stackoverflow.com/questions/6503424/how-to-programmatically-set-the-image-source
            BitmapImage image = new BitmapImage(new Uri(imagePaths[SongList.SelectedIndex]));
            //assigns image path to image box
            AlbumArt.Source = image;

            //player plays song
            musPlayer.Play();
        }

        //function to move player back one song
        private void PreviousSongButton_Click(object sender, RoutedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            //sets selected index backwards 1
            SongList.SelectedIndex = SongList.SelectedIndex - 1;

            //creates new uri based on new selected song index
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            //player plays song
            musPlayer.Play();
        }

        //function to shuffle songs in SongList, based on comment found on this site http://csharphelper.com/blog/2014/07/randomize-arrays-in-c/
        //originally tried to just create a new array and have it set to random values of the orignal title array, but did some more searching for a different way and found this
        private void ShuffleSongsButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Close();

            //creates random for shuffling song list
            Random rand = new Random();

            //creates list to hold titles to be shuffled
            List<string> shufTitle = new List<string>();

            //adds song titles to list
            foreach (string title in SongList.Items)
            {
                shufTitle.Add(title);
            }

            //creates variable of randomly selected song titles
            var randomTitleList = from i in shufTitle
                                  orderby rand.Next()
                                  select i;

            //clears current listbox
            SongList.Items.Clear();

            //adds randomTitleList to listbox
            foreach (string title in randomTitleList)
            {
                SongList.Items.Add(title);
            }
        }

        //function to move player forward one song
        private void NextSongButton_Click(object sender, RoutedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            //sets selected index forward 1
            SongList.SelectedIndex = SongList.SelectedIndex + 1;

            //creates new uri based on new selected song index
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            //player plays song
            musPlayer.Play();
        }

        //function to add art for songs, functions in same way as adding songs
        private void AddSongArtButton_Click(object sender, RoutedEventArgs e)
        {
            //opens file window to add images
            OpenFileDialog file = new OpenFileDialog();

            //filters selection for only music
            file.Filter = file.Filter = "All Supported Images | *.jpg; *.png | JPGs | *.jpg | PNGs | *.png";
            //allows for selection of multiple images
            file.Multiselect = true;

            //adds image path to imagePaths array
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imagePaths = file.FileNames;
            }
        }
    }
}
