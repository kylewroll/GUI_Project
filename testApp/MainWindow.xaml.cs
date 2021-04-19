/* Authors: Kyle Rolland, Andrew Marshall
 * Date: 4/1/2021
 * File: MainWindow.cs
 * Description: Contains global variables used throughout the application, initialized application, contains smaller functions that don't have a good place to be put
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Design and code for adding/playing music based off of tutorial by Vijay Thapa: https://www.youtube.com/watch?v=wjmU28ukjwA

        //creates music player
        MediaPlayer musPlayer = new MediaPlayer();

        //array for song titles
        string[] songTitles = new string[0];
        //array for paths to song files in folder
        string[] songPaths = new string[0];
        //array for paths to image files in folder
        string[] imagePaths = new string[0];

        string[] titleTemp;
        string[] songPathTemp;

        //boolean for determining whether or not images are added
        bool images = false;

        //boolean for determining whether songLabel is clicked, starts out false
        bool songLabelClick = false;

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

        //function called when light theme button is pressed
        private void LightThemeButton_Click(object sender, RoutedEventArgs e)
        {
            //creates light theme user interface
            UserInterface lightTheme = new CreateLightUserInterface();

            //returned values aren't used, just needed them to properly access the functions to make the changes
            UIBackground lightBackground = lightTheme.MakeBackground(mainScreen);
            Toolbar lightToolbar = lightTheme.MakeToolbar(ButtonBar);
            SongLabel lightSongLabel = lightTheme.MakeSongLabel(CurrentSongLabel);
        }

        //function called when dark theme button is pressed
        private void DarkThemeButton_Click(object sender, RoutedEventArgs e)
        {
            //creates dark theme user interface
            UserInterface darkTheme = new CreateDarkUserInterface();

            UIBackground darkBackground = darkTheme.MakeBackground(mainScreen);
            Toolbar darkToolbar = darkTheme.MakeToolbar(ButtonBar);
            SongLabel darkSongLabel = darkTheme.MakeSongLabel(CurrentSongLabel);
        }

        //function that is called when the label is left clicked, if it's not displaying song information, the label swaps to showing the info. If it is showing the info, it swaps back to
        //showing the name from the song list
        private void CurrentSongLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //store index for easier user
            int selection = SongList.SelectedIndex;

            //swap back to song list name, and change boolean so that it will display info on next click
            if (songLabelClick)
            {
                string songName = songTitles[selection];
                CurrentSongLabel.Content = "Currently playing: " + songName;

                songLabelClick = false;

                return;
            }


            if (images == false)
            {
                CurrentSongLabel.Content = "Please select add an image to this song, and try again.";
                songLabelClick = true;
                return;
            }

            //create info display interface
            InfoDisplay showSongInfo = new SongInfoDisplay();

            //call print info from SongInfoDisplay
            showSongInfo.PrintInfo(CurrentSongLabel);

            //use showSongInfo to create new decorated info item, function built to work with images
            DecoratedInfo decoratedInfo = new DecoratedInfo(showSongInfo, CurrentSongLabel, songTitles[selection], songPaths[selection], imagePaths[selection]);

            //call decorated info's functions to update label
            decoratedInfo.GetSongTitle();
            decoratedInfo.GetSongPath();
            decoratedInfo.GetImagePath();

            //change boolean to it will display song list name on next click
            songLabelClick = true;
        }
    }
}
