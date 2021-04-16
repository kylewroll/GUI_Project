/* Authors: Kyle Rolland, Andrew Marshall
 * Date: 4/16/2021
 * File: Song_Label_Decorator.cs
 * Description: Contains decorator pattern for the current song label
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
        //this one of the few ways we could try to implement a decorator in a somewhat useful way, and even then this could easily just be changed into one function,
        //instead of this mess, but the label is "decorated" with some extra functionality

        //abstract info display, overwritten by concrete child
        interface InfoDisplay
        {
            void PrintInfo(System.Windows.Controls.Label label);
        }

        //concrete child, implements print info
        class SongInfoDisplay : InfoDisplay
        {

            void InfoDisplay.PrintInfo(System.Windows.Controls.Label label)
            {
                label.Content = "Information: \n";
            }
        }

        //abstract decorator, to give the label "responsibilities"
        abstract class LabelDecorator : InfoDisplay
        {
            //used to call parent function
            InfoDisplay display;

            public LabelDecorator(InfoDisplay song)
            {
                display = song;
            }

            //aforementioned call to parent function
            void InfoDisplay.PrintInfo(System.Windows.Controls.Label label)
            {
                display.PrintInfo(label);
            }
        }

        //concrete implementation of label decorator
        class DecoratedInfo : LabelDecorator
        {
            //used for "decorating" the song information
            System.Windows.Controls.Label label;
            string songTitle;
            string songPath;
            string imagePath;

            public DecoratedInfo(InfoDisplay song, System.Windows.Controls.Label curLabel,  string curTitle, string curSongPath, string curImagePath) : base(song)
            {
                label = curLabel;
                songTitle = curTitle;
                songPath = curSongPath;
                imagePath = curImagePath;
            }

            //retrieves and adds song title without file extension to label
            public void GetSongTitle()
            {
                int takeUntil = songTitle.IndexOf(".mp3");

                string titleNoExt = songTitle.Substring(0, takeUntil);

                label.Content += "Song Title: " + titleNoExt + '\n';
            }

            //adds song path to label
            public void GetSongPath()
            {
                label.Content += "Song Path: " + songPath + '\n';
            }

            //adds image path to label
            public void GetImagePath()
            {
                label.Content += "Image Path: " + imagePath + '\n';
            }
        }
    }
}
