using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;
using System.Text.Json;


namespace MusicMi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*private MediaPlayer player = new MediaPlayer();
        private ObservableCollection<Song> songs = new ObservableCollection<Song>();
        private int currentIndex = -1;
        private DispatcherTimer timer;
        private bool shuffle = false;
        private bool repeat = false;
        private Random random = new Random();*/

        public MainWindow()
        {
            InitializeComponent();
            /*Playlist.ItemsSource = songs;

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;

            player.MediaEnded += Player_MediaEnded;*/
        }

        /*private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "MP3 Files|*.mp3",
                Multiselect = true
            };

            if (dlg.ShowDialog() == true)
            {
                foreach (string file in dlg.FileNames)
                {
                    var song = LoadSongMetadata(file);
                    songs.Add(song);
                }
            }
        }

        private Song LoadSongMetadata(string filePath)
        {
            try
            {
                var tagFile = TagLib.File.Create(filePath);
                var song = new Song
                {
                    FilePath = filePath,
                    Title = tagFile.Tag.Title ?? System.IO.Path.GetFileName(filePath),
                    Artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist",
                    Album = tagFile.Tag.Album ?? "Unknown Album"
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

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Playlist.SelectedIndex >= 0)
            {
                currentIndex = Playlist.SelectedIndex;
                PlaySong(songs[currentIndex]);
            }
        }

        private void PlaySong(Song song)
        {
            player.Open(new Uri(song.FilePath));
            player.Play();
            timer.Start();

            SongTitle.Text = song.Title;
            ArtistName.Text = song.Artist;
            AlbumName.Text = song.Album;
            AlbumArt.Source = song.AlbumArt;
        }

        private void Pause_Click(object sender, RoutedEventArgs e) => player.Pause();
        private void Stop_Click(object sender, RoutedEventArgs e) => player.Stop();

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
                PlaySong(songs[--currentIndex]);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (shuffle)
                currentIndex = random.Next(songs.Count);
            else if (currentIndex < songs.Count - 1)
                currentIndex++;
            else if (repeat)
                currentIndex = 0;
            else
                return;

            PlaySong(songs[currentIndex]);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (player.Source != null && player.NaturalDuration.HasTimeSpan)
            {
                ProgressSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = player.Position.TotalSeconds;
            }
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Math.Abs(player.Position.TotalSeconds - ProgressSlider.Value) > 1)
            {
                player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = VolumeSlider.Value;
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e) => shuffle = !shuffle;
        private void Repeat_Click(object sender, RoutedEventArgs e) => repeat = !repeat;

        private void Player_MediaEnded(object sender, EventArgs e) => Next_Click(null, null);

        private void SavePlaylist_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "JSON Playlist|*.json" };
            if (dlg.ShowDialog() == true)
            {
                var json = JsonSerializer.Serialize<object>(songs);
                File.WriteAllText(dlg.FileName, json);
            }
        }

        private void LoadPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "JSON Playlist|*.json" };
            if (dlg.ShowDialog() == true)
            {
                var json = File.ReadAllText(dlg.FileName);
                var loadedSongs = JsonSerializer.Deserialize<ObservableCollection<Song>>(json);
                songs.Clear();
                foreach (var song in loadedSongs) songs.Add(song);
            }
        }*/

    }
    public class Song
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public BitmapImage AlbumArt { get; set; }
    }
}
