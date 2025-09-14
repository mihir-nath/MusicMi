using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MusicMi.Model;
using MusicMi.Services;

namespace MusicMi
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MusicPlayer _player;
        public event PropertyChangedEventHandler? PropertyChanged;
        private Song? _song;
        public ObservableCollection<Song> songs = new ObservableCollection<Song>();
        private double _progress;
        private bool _isPlaying;
        private double _volume;



        public MainViewModel() { 
            _player= new MusicPlayer();
            /*LoadMusicCommand = new RelayCommand(() =>
            {
                LoadSongsFromDirectory();
            });*/
            OpenFileCommand = new RelayCommand((_) => { _player.UploadMusicFromFile(); });
            PlayPauseCommand = new RelayCommand((_) => {
                if (IsPlaying) {
                    _player.StopSong();
                }
                else
                {
                    _player.PlaySong(Song);
                }
                     
            });
            NextCommand = new RelayCommand((_) => { _player.NextSong(); });
            PreviousCommand = new RelayCommand((_) => { _player.PreviousSong(); });
            RepeatCommand = new RelayCommand((_) => { _player.Resume(); });
            _player.ProgressChanged += (p) => Progress = p;
            _player.SongChanged += (s) => Song = s;
            _player.PlayingStateChanged += (isPlaying) => IsPlaying = isPlaying;
            LoadSongsFromDirectory();
        }
        

        private void LoadSongsFromDirectory()
        {
            songs.Clear();
            foreach (var song in _player.LoadSongs(@"C:\Users\MNH5\Downloads"))
            {
                songs.Add(song);
            }
            Song = songs.FirstOrDefault();
        }
        public Song Song 
        { 
            get { return _song; }
            set { _song = value;
                OnPropertyChanged(nameof(Song));
            }
        }
        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        public ObservableCollection<Song> Songs
        {
            get { return songs; }
            set
            {
                songs = value;
                OnPropertyChanged(nameof(Songs));
            }
        }
        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }
        //public ICommand LoadMusicCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand PlayPauseCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand RepeatCommand { get; }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
