/* Authors: Kyle Rolland, Andrew Marshall
 * Date: 4/6/2021
 * File: Resume_Pause_Volume.cs
 * Description: Contains functions that relate to The pause/resume buttons and volume buttons
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
using System.Windows.Controls.Primitives;

namespace testApp
{
    public partial class MainWindow : Window
    {
        //function to resume playing current song
        private void ResumePlayButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Play();

            timer.Start();
        }

        //function to pause current song
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            musPlayer.Pause();

            timer.Stop();
        }

        //function to update volume based on volumer slider position
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musPlayer.Volume = e.NewValue;
        }
    }
}
