/* Authors: Kyle Rolland, Andrew Marshall
 * Date: 4/15/2021
 * File: Interface_Factory.cs
 * Description: Contains abstract and concrete implementations of the user interface classes that create and call the Theme_Creator functions
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
        //abstract UserInterface interface
        interface UserInterface
        {
            //passes application grid to background creator
            UIBackground MakeBackground(Grid grid);
            //passes toolbar to toolbar creator
            Toolbar MakeToolbar(System.Windows.Controls.ToolBar bar);
            //passes song label to song label creator
            SongLabel MakeSongLabel(System.Windows.Controls.Label curSong);
        }
        
        //concrete create light user interface implementation 
        class CreateLightUserInterface : UserInterface
        {
            UIBackground UserInterface.MakeBackground(Grid grid)
            {
                UIBackground lightBackground = new LightBackground();
                lightBackground.CreateBackground(grid);
                return lightBackground;
            }

            Toolbar UserInterface.MakeToolbar(System.Windows.Controls.ToolBar bar)
            {
                Toolbar lightToolbar = new LightToolbar();
                lightToolbar.CreateToolbar(bar);
                return lightToolbar;
            }

            SongLabel UserInterface.MakeSongLabel(System.Windows.Controls.Label curSong)
            {
                SongLabel lightSongLabel = new LightSongLabel();
                lightSongLabel.CreateSongLabel(curSong);
                return lightSongLabel;
            }
        }

        //concrete create dark user interface implementation
        class CreateDarkUserInterface : UserInterface
        {
            UIBackground UserInterface.MakeBackground(Grid grid)
            {
                UIBackground darkBackground = new DarkBackground();
                darkBackground.CreateBackground(grid);
                return darkBackground;
            }

            Toolbar UserInterface.MakeToolbar(System.Windows.Controls.ToolBar bar)
            {
                Toolbar darkToolbar = new DarkToolbar();
                darkToolbar.CreateToolbar(bar);
                return darkToolbar;
            }

            SongLabel UserInterface.MakeSongLabel(System.Windows.Controls.Label curSong)
            {
                SongLabel darkSongLabel = new DarkSongLabel();
                darkSongLabel.CreateSongLabel(curSong);
                return darkSongLabel;
            }
        }
    }
}
