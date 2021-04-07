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

        //function to decrease volume by .1, and update volume label, unless volume is at 0
        private void VolumeDownButton_Click(object sender, RoutedEventArgs e)
        {

            if (musPlayer.Volume == 0)
            {
                musPlayer.Volume = 0;
            }

            else
            {
                musPlayer.Volume -= .1;
            }

            VolumeLabel.Content = musPlayer.Volume;
        }

        //function to increase volume by .1, and update volume label, unless volume is at 1
        private void VolumeUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (musPlayer.Volume == 1)
            {
                musPlayer.Volume = 1;
            }

            else
            {
                musPlayer.Volume += .1;
            }

            VolumeLabel.Content = musPlayer.Volume;
        }
    }
}
