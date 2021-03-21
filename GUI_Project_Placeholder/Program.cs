using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Project_Placeholder
{
    class Song
    {
        string songName;
        string artistName;
        double songLength;

        public void Set_Name(string songName)
        {
            this.songName = songName;
        }

        public void Set_Artist(string artistName)
        {
            this.artistName = artistName;
        }

        public void Set_Length(double songLength)
        {
            this.songLength = songLength;
        }

        public void Print()
        {
            Console.WriteLine("Song Name: " + songName);
            Console.WriteLine("Artist Name: " + artistName);
            Console.WriteLine("Song Length: " + songLength);
        }
    }

    
    class Song_Queue
    {
        public List<Song> songs = new List<Song>();
    }
    

    class Program
    {
        static void Main(string[] args)
        {
            Song addSong = new Song();
            List<Song> songs = new List<Song>();
            //Song_Queue songQueue = new Song_Queue();

            addSong.Set_Name("Shibuya Shuffle");
            addSong.Set_Artist("Engelwood");
            addSong.Set_Length(2.08);

            songs.Add(addSong);

            for (int i = 0; i < songs.Count; i++)
            {
                songs[i].Print();
            }

            Console.ReadKey();
        }
    }
}
