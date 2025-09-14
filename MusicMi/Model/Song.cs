using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MusicMi.Model
{
    public class Song
    {
        public string? FilePath { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public BitmapImage? AlbumArt { get; set; }
        public string? AlbumDuration { get; set;}
    }
}
