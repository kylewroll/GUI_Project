/* Authors: Kyle Rolland, Andrew Marshall
 * Date: 4/15/2021
 * File: Theme_Creator.cs
 * Description: Contains abstract and concrete implementation of the items that are changed when swapping themes
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
        //abstract background interface
        interface UIBackground
        {
            void CreateBackground(Grid grid);
        }

        //concrete light backgorund implementation
        class LightBackground : UIBackground
        {
            void UIBackground.CreateBackground(Grid grid)
            {
                grid.Background = Brushes.White;
            }
        }

        //concrete dark background implementation
        class DarkBackground : UIBackground
        {
            void UIBackground.CreateBackground(Grid grid)
            {
                grid.Background = Brushes.DarkGray;
            }
        }

        //abstract toolbar interface
        interface Toolbar
        {
            void CreateToolbar(System.Windows.Controls.ToolBar bar);
        }

        //concrete light toolbar implementation
        class LightToolbar : Toolbar
        {
            void Toolbar.CreateToolbar(System.Windows.Controls.ToolBar bar)
            {
                bar.Background = Brushes.White;
            }
        }

        //concrete dark toolbar implementation
        class DarkToolbar : Toolbar
        {
            void Toolbar.CreateToolbar(System.Windows.Controls.ToolBar bar)
            {
                bar.Background = Brushes.DarkGray;
            }
        }

        //abstract song label interface
        interface SongLabel
        {
            void CreateSongLabel(System.Windows.Controls.Label curSong);
        }

        //concrete light song label implementation
        class LightSongLabel : SongLabel
        {
            void SongLabel.CreateSongLabel(System.Windows.Controls.Label curSong)
            {
                curSong.Background = Brushes.White;
                curSong.Foreground = Brushes.Black;
            }
        }

        //concrete dark song label implementation
        class DarkSongLabel : SongLabel
        {
            void SongLabel.CreateSongLabel(System.Windows.Controls.Label curSong)
            {
                curSong.Background = Brushes.DarkGray;
                curSong.Foreground = Brushes.White;
            }
        }
    }
}
