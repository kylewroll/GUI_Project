/* Authors: Kyle Rolland, Andrew Marshall
 * Date: 4/6/2021
 * File: Navigation_Sorting.cs
 * Description: Contains functions that relate to sorting or moving songs, or moving through song list
 */

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
        }

        //function to move player forward one song
        private void NextSongButton_Click(object sender, RoutedEventArgs e)
        {
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

        //function that moves selected song up one position in the list
        private void MoveSongUp_Click(object sender, RoutedEventArgs e)
        {
            //do nothing if song is at top of list
            if(SongList.SelectedIndex == 0)
            {
                return;
            }

            else
            {
                //store selection to be updated at end
                int selection = SongList.SelectedIndex;

                //update song titles array
                string songTitleTemp = songTitles[selection - 1];
                songTitles[selection - 1] = songTitles[selection];
                songTitles[selection] = songTitleTemp;

                //update song paths array
                string songPathTemp = songPaths[selection - 1];
                songPaths[selection - 1] = songPaths[selection];
                songPaths[selection] = songPathTemp;

                //update image paths array
                if (images)
                {
                    string imagePathTemp = imagePaths[selection - 1];
                    imagePaths[selection - 1] = imagePaths[selection];
                    imagePaths[selection] = imagePathTemp;
                }

                //clear and repopulate song list
                SongList.Items.Clear();

                for (int i = 0; i < songTitles.Length; i++)
                {
                    SongList.Items.Add(songTitles[i]);
                }

                //adjust selected index down one
                SongList.SelectedIndex = selection - 1;
            }

        }

        //function that moves selected song down one position in the list
        private void MoveSongDown_Click(object sender, RoutedEventArgs e)
        {
            //do nothing if song is at bottom of list
            if (SongList.SelectedIndex == SongList.Items.Count - 1)
            {
                return;
            }

            else
            {
                //index stored so that the selection can be adjusted after moving
                int selection = SongList.SelectedIndex;

                //update song title array
                string songTitleTemp = songTitles[selection + 1];
                songTitles[selection + 1] = songTitles[selection];
                songTitles[selection] = songTitleTemp;

                //update song path array
                string songPathTemp = songPaths[selection + 1];
                songPaths[selection + 1] = songPaths[selection];
                songPaths[selection] = songPathTemp;

                //update image path array
                if (images)
                {
                    string imagePathTemp = imagePaths[selection + 1];
                    imagePaths[selection + 1] = imagePaths[selection];
                    imagePaths[selection] = imagePathTemp;
                }

                //clear and repopulate song list
                SongList.Items.Clear();

                for (int i = 0; i < songTitles.Length; i++)
                {
                    SongList.Items.Add(songTitles[i]);
                }

                //adjust selected index up one
                SongList.SelectedIndex = selection + 1;
            }
        }

        //function to clear lisbox
        private void ClearListButton_Click(object sender, RoutedEventArgs e) 
        {
            //close player
            musPlayer.Close();
            //stop timer
            timer.Stop();
            //clear image
            AlbumArt.Source = null;
            //clear song label
            CurrentSongLabel.Content = " ";
            //clear song clock
            SongDurationClock.Text = " ";
            //clear total song length
            TotalSongLength.Text = " ";
            //reset song progress bar
            SongProgressBar.Value = 0;

            //clear arrays
            Array.Clear(songTitles, 0, songTitles.Length);
            Array.Clear(songPaths, 0, songPaths.Length);

            if(images)
            {
                Array.Clear(imagePaths, 0, imagePaths.Length);
            }
           
            //clear list
            SongList.Items.Clear();
        }
    }
}
