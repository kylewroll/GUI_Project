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
    public partial class MainWindow : Window
    {
        //function to play currently selected song, and display corresponding art
        public void SongList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void PlaySongButton_Click(object sender, RoutedEventArgs e)
        {
            //ends any previously playing songs
            musPlayer.Close();

            timer.Stop();
            start = DateTime.Now;

            //creates new uri based on selected song
            Uri music = new Uri(songPaths[SongList.SelectedIndex]);
            //player opens uri
            musPlayer.Open(music);

            if (images)
            {
                //conversion based on https://stackoverflow.com/questions/6503424/how-to-programmatically-set-the-image-source
                BitmapImage image = new BitmapImage(new Uri(imagePaths[SongList.SelectedIndex]));
                //assigns image path to image box
                AlbumArt.Source = image;
            }


            //sets songName variable, and updates CurrentSongLabel to display it
            string songName = songTitles[SongList.SelectedIndex];
            CurrentSongLabel.Content = "Currently selected: " + songName;

            //player plays song
            musPlayer.Play();

            //starts timer
            timer.Start();
        }

        //function to play all songs from song list, manually plays first song, then loops through rest till end
        private void PlayAllSongsButton_Click(object sender, RoutedEventArgs e)
        {
            //sets index, so song is visibly selected while playing
            SongList.SelectedIndex = 0;

            timer.Stop();
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
            if (currentSongIndex < SongList.Items.Count)
            {
                //updates index in listbox
                SongList.SelectedIndex = currentSongIndex;

                timer.Stop();
                start = DateTime.Now;

                if (images)
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
    }
}
