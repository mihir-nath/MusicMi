using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        private double _volume = 0.5;



        public MainViewModel() { 
            _player= new MusicPlayer();
            /*LoadMusicCommand = new RelayCommand(() =>
            {
                LoadSongsFromDirectory();
            });*/
            OpenFileCommand = new RelayCommand((_) => { _player.UploadMusicFromFile(); });
            PlayPauseCommand = new RelayCommand((_) => {
                PlayPauseClicked();       
            });
            NextCommand = new RelayCommand((_) => { _player.NextSong(); });
            PreviousCommand = new RelayCommand((_) => { _player.PreviousSong(); });
            RepeatCommand = new RelayCommand((_) => { _player.Resume(); });
            SeekCommand = new RelayCommand((p) =>
            {
                if (p is double value)
                {
                    _player.Seek(value);
                }
            });
            _player.ProgressChanged += (p) => Progress = p;
            //_player.SongChanged += (s) => Song = s;
            _player.PlayingStateChanged += (isPlaying) => IsPlaying = isPlaying;
            _player.SongChanged += (s) => OnSongChanged(s);
            LoadSongsFromDirectory();
        }
        

        private void LoadSongsFromDirectory()
        {
            songs.Clear();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Songs");
            foreach (var song in _player.LoadSongs(path))
            {
                songs.Add(song);
            }
            //Song = songs.FirstOrDefault();
        }
        public Song Song 
        { 
            get { return _song; }
            set { 
                _song = value;
                if(_song != null)
                {
                    _player.PlaySong(_song);
                }
                OnPropertyChanged(nameof(Song));
            }
        }
        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                _player.SetVolume(Volume);
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
        public ICommand SeekCommand { get; }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnSongChanged(Song song)
        {
            // Update property without triggering PlaySong again
            _song = song;
            OnPropertyChanged(nameof(Song));
        }
        public void SeekTo(double position)
        {
            _player.Seek(position);
        }
        public void PlayPauseClicked() {
            if (Song == null && Songs !=null)
            {
                Song = Songs.FirstOrDefault();

            }
            else
            {
                if (IsPlaying)
                {
                    _player.StopSong();
                }
                else
                {
                    //_player.PlaySong(Song);
                    _player.Resume();
                }
            }
        }
    }
}
