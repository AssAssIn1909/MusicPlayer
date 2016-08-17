using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class Song
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int? Track { get; set; }
        public string Path { get; set; }
    }
}
