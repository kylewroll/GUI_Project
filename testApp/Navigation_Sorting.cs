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
            if (SongList.SelectedIndex == SongList.Items.Count - 1)
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

                if (images == true)
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

        //function to sort songs in list by title
        private void SortByTitleButton_Click(object sender, RoutedEventArgs e)
        {
            SongList.Items.Clear();


            for (int i = 0; i < songTitles.Length; i++)
            {
                for (int j = 1; j < songTitles.Length - i; j++)
                {
                    if (String.Compare(songTitles[j - 1], songTitles[j]) > 0)
                    {
                        //replace song titles
                        string titleTemp = songTitles[j - 1];
                        songTitles[j - 1] = songTitles[j];
                        songTitles[j] = titleTemp;

                        //replace song paths (so player doesnt play song originally at [0])
                        string songPathTemp = songPaths[j - 1];
                        songPaths[j - 1] = songPaths[j];
                        songPaths[j] = songPathTemp;

                        if (images == true)
                        {
                            //replace image paths (so images are properly updated)
                            string imagePathTemp = imagePaths[j - 1];
                            imagePaths[j - 1] = imagePaths[j];
                            imagePaths[j] = imagePathTemp;
                        }
                    }
                }
            }

            for (int i = 0; i < songTitles.Length; i++)
            {
                //add song titles to listbox in new order
                SongList.Items.Add(songTitles[i]);
            }
        }

        //function to clear lisbox
        private void ClearListButton_Click(object sender, RoutedEventArgs e) 
        {
            Array.Clear(songTitles, 0, songTitles.Length);
            Array.Clear(songPaths, 0, songPaths.Length);

            if(images)
            {
                Array.Clear(imagePaths, 0, imagePaths.Length);
            }
           

            SongList.Items.Clear();
        }
    }
}
