/* Authors: Kyle Rolland, Andrew Marshall
 * Date: 4/6/2021
 * File: Add_Export.cs
 * Description: Contains functions that relate to adding songs, images, etc, or moving them
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
        //function to add songs to listbox
        private void AddSongsButton_Click(object sender, RoutedEventArgs e)
        {
            //clears song list, so previously added songs don't stay on the list to be selected, and break things when chosen
            SongList.Items.Clear();

            //opens file window to add songs
            OpenFileDialog file = new OpenFileDialog();

            //filters selection to only mp3, wma
            file.Filter = file.Filter = "All Supported Audio | *.mp3; *.wma | MP3s | *.mp3 | WMAs | *.wma";
            //allows for selection of multiple songs
            file.Multiselect = true;
            
            //adds song name and path to their respective arrays
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tempTitles = file.SafeFileNames;
                tempSongPaths = file.FileNames;
            }

            //if array is default, set it equal to temp
            if (songTitles.Length == 1000)
            {
                songTitles = tempTitles;
                songPaths = tempSongPaths;
            }

            //otherwise resize arrays and copy temp to the new array, allows user to select multiple files to add, and properly adds them. kinda of tough to figure out, and janky, seems like c#
            //was built around lists, so doing this with arrays was a hassle
            else
            {
                int oldLength = songTitles.Length;

                Array.Resize(ref songTitles, oldLength + tempTitles.Length);
                Array.Copy(tempTitles, 0, songTitles, oldLength, tempTitles.Length);

                Array.Resize(ref songPaths, oldLength + tempSongPaths.Length);
                Array.Copy(tempSongPaths, 0, songPaths, oldLength, tempTitles.Length);
            }
            
            //prevents crashing when closing dialog without selection
            if (songTitles == null)
            {
                return;
            }

            //add items to list box
            for (int i = 0; i < songTitles.Length; i++)
            {
                SongList.Items.Add(songTitles[i]);
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
                tempImagePaths = file.FileNames;
            }

            //if array is default, set it equal to temp
            if (imagePaths.Length == 1000)
            {
                imagePaths = tempImagePaths;
            }

            //otherwise resize and copy
            else
            {
                int oldLength = imagePaths.Length;

                Array.Resize(ref imagePaths, oldLength + tempImagePaths.Length);
                Array.Copy(tempImagePaths, 0, imagePaths, oldLength, tempImagePaths.Length);
            }

            //prevents program from crashing if you close out of file dialog without selecting
            if(imagePaths == null)
            {
                return;
            }

            //set images to true
            images = true;
        }

        //function for changing the art of a specific song
        private void Change_Image_Button_Click(object sender, RoutedEventArgs e)
        {
            //if no song selected, return
            if (SongList.SelectedIndex == -1)
            {
                return;
            }

            //store for readability
            int selection = SongList.SelectedIndex;

            //opens file window to add images
            OpenFileDialog file = new OpenFileDialog();

            //filters selection to only jpgs, pngs
            file.Filter = file.Filter = "All Supported Images | *.jpg; *.png | JPGs | *.jpg | PNGs | *.png";
            //prevents selection of multiple images
            file.Multiselect = false;

            //overwrite array value at selection, set images to true
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imagePaths[selection] = file.FileName;
                images = true;
            }

            //more crash prevention
            if (imagePaths == null)
            {
                return;
            }
        }

        //function to save the songs in the listbox to an outside folder, currently saves as XAML files
        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            //create new writer for outputting
            StreamWriter writer;
            //open dialog to save files
            SaveFileDialog save = new SaveFileDialog();

            //makes it so that any previously selected files are not displayed first
            save.RestoreDirectory = false;
            //saves as xaml file
            save.Filter = "XML File | *.xml | All files | *.*";

            if(save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //initialize writer to wherever user chooses to save
                writer = new StreamWriter(save.FileName);

                //loop and print out titles, paths, images
                for(int i = 0; i < SongList.Items.Count; i++)
                {
                    writer.WriteLine(songTitles[i]);
                    writer.WriteLine(songPaths[i]);

                    if(images)
                    {
                        writer.WriteLine(imagePaths[i]);
                    }
                }
                //close writer
                writer.Close();
            }
        }

        //function to load a playlist. if save playlist can save music/image files separately, this could be removed, and songs could be added by the add button. right now, reads in xaml file
        //and adds song path to listbox
        private void LoadPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            //songNum for storing data properly
            int songNum = 0;

            //create reader to read in file data
            StreamReader reader;
            //create dialog to load files
            OpenFileDialog load = new OpenFileDialog();
            //prevent selecting multiple playlists in one session
            load.Multiselect = false;
            
            if(load.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //initialize reader with selected loading destination
                reader = new StreamReader(load.FileName);

                //run loop while file still has contents
                while(reader.Peek() >= 0)
                {
                    //store line's info at songNum's index
                    songTitles[songNum] = reader.ReadLine();
                    songPaths[songNum] = reader.ReadLine();

                    //read in images, if images have been previously added in this session. Maybe can find way to see if file has .jpg, which would allow the user to launch the app, and load
                    //playlists immediately, and the app handles whether or not there are images in the file
                    if(images)
                    {
                        imagePaths[songNum] = reader.ReadLine();
                    }

                    //increase song number and run again
                    songNum++;
                }

                //close reader
                reader.Close();

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

                //resize arrays, in case playlist has less songs previously added
                Array.Resize(ref songTitles, songNum);
                Array.Resize(ref songPaths, songNum);

                if (images)
                {
                    Array.Resize(ref imagePaths, songNum);
                }

                //clear list
                SongList.Items.Clear();

                //store loaded info into list box
                for (int i = 0; i < songTitles.Length; i++)
                {
                    SongList.Items.Add(songTitles[i]);
                }
            }
        }
    }
}

