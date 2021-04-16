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
            //opens file window to add songs
            OpenFileDialog file = new OpenFileDialog();

            //filters selection to only mp3, wma
            file.Filter = file.Filter = "All Supported Audio | *.mp3; *.wma | MP3s | *.mp3 | WMAs | *.wma";
            //allows for selection of multiple songs
            file.Multiselect = true;

            //adds song name and path to their respective arrays
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                songTitles = (file.SafeFileNames);
                songPaths = file.FileNames;
            }

            //adds song names to listbox
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
                imagePaths = file.FileNames;
                images = true;
            }
        }

        //function to save the songs in the listbox to an outside folder, currently saves as XAML files, goal is to see if it can save music files and images separately,
        //but at the same time preferably
        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            //create new writer for outputting
            StreamWriter writer;
            //open dialog to save files
            SaveFileDialog save = new SaveFileDialog();
            //makes it so that any previously selected files are not displayed first
            save.RestoreDirectory = false;
            //saves as either xaml file, or file
            save.Filter = "XML File | *.xml | All files | *.*";

            if(save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //initialize writer to wherever is chosed to save
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

                while(reader.Peek() >= 0)
                {
                    //add info to list, need this to be song paths to work?
                    SongList.Items.Add(reader.ReadLine());
                }
            }
        }
    }
}

