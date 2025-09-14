using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Media;
using MusicMi.Model;
using System.Windows.Threading;

namespace MusicMi.Services
{
    public class MusicPlayer
    {
        private ObservableCollection<Song> playList = new ObservableCollection<Song>();
        private MediaPlayer player = new MediaPlayer();
        private DispatcherTimer timer;
        private int currentIndex = -1;
        private bool isRepeating = false;

        public event Action<double>? ProgressChanged;
        public event Action<Song?>? SongChanged;
        public event Action<bool>? PlayingStateChanged;

        public MusicPlayer() { 
            timer= new DispatcherTimer();
            timer.Tick += (s, e) =>
            {
                if(player.Source!= null && player.NaturalDuration.HasTimeSpan)
                {
                    double progress = player.Position.TotalSeconds / player.NaturalDuration.TimeSpan.TotalSeconds;
                    ProgressChanged?.Invoke(progress);
                }
            };

            player.MediaEnded += (s, e) =>
            {
                if(isRepeating && currentIndex > 0)
                {
                    PlaySong(playList[currentIndex]);
                }
                else
                {
                    NextSong();
                }
            };
        }
        public void UploadMusicFromFile()
        {
            Console.WriteLine("Open Folder");
            OpenFileDialog dlg = new OpenFileDialog {
                Filter = "MP3 Files |* .mp3",
                Multiselect = true,
            };
            if(dlg.ShowDialog() == true )
            {
                string songsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Songs");
                if (!Directory.Exists(songsFolder))
                    Directory.CreateDirectory(songsFolder);

                foreach ( string file in dlg.FileNames )
                {
                    try
                    {
                        string destFile = Path.Combine(songsFolder, Path.GetFileName(file));
                        if (!File.Exists(destFile))
                        {
                            File.Copy(file, destFile, overwrite: false);
                        }
                        var song = LoadSongMetaData(file);
                        playList.Add(song);
                    } catch
                    {
                        Console.WriteLine("Error uploading File");
                    }
                    
                }
            }
        }
        private  Song LoadSongMetaData(string filePath)
        {
            try
            {
                var tagFile = TagLib.File.Create(filePath);
                var song = new Song
                {
                    FilePath = filePath,
                    Title = tagFile.Tag.Title ?? System.IO.Path.GetFileName(filePath),
                    Artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist",
                    Album = tagFile.Tag.Album ?? "Unknown Album",
                    AlbumDuration = tagFile.Tag.Length ?? "00:00"
                };

                if (tagFile.Tag.Pictures.Any())
                {
                    var bin = (byte[])(tagFile.Tag.Pictures[0].Data.Data);
                    using var ms = new MemoryStream(bin);
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.StreamSource = ms;
                    img.EndInit();
                    song.AlbumArt = img;
                }
               
                return song;
            }
            catch
            {
                return new Song
                {
                    FilePath = filePath,
                    Title = System.IO.Path.GetFileName(filePath),
                    Artist = "Unknown Artist",
                    Album = "Unknown Album"
                };
            }
        }
        public ObservableCollection<Song> LoadSongs(string filePath)
        {
            var files = Directory.GetFiles(filePath, "*.mp3");
            foreach (var file in files)
            {
                var song = LoadSongMetaData(file);
                playList.Add(song);
            }
            return playList;
        }
        public void PlaySong(Song song)
        {
            int index = playList.IndexOf(song);
            if (index > 0 ) { currentIndex= index; }
            player.Open(new Uri(song.FilePath));
            player.Play();
            timer.Start();
            SongChanged?.Invoke(song);
            PlayingStateChanged?.Invoke(true);
        }
        public void StopSong()
        {
            player.Pause();
            PlayingStateChanged?.Invoke(false);
        }
        public void Resume()
        {
            player.Play();
            PlayingStateChanged?.Invoke(true);
        }
        public void NextSong()
        {
            if(playList.Count == 0) return;
            currentIndex = (currentIndex + 1) % playList.Count;
            PlaySong(playList[currentIndex]);
        }
        public void PreviousSong()
        {
            if (playList.Count == 0) return;
            currentIndex = (currentIndex - 1 + playList.Count) % playList.Count;
            PlaySong(playList[currentIndex]);
        }
        public void Seek(double progress)
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                var position =TimeSpan.FromSeconds(player.NaturalDuration.TimeSpan.TotalSeconds * progress);
                player.Position= position;
            }
        }
        public void SetVolume(double volume)
        {
            player.Volume = volume;
        }
    }
    
}
