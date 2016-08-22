using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.Tags.ID3;
using MusicPlayer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Controls;

namespace MusicPlayer
{
    /// <summary>
    /// Music player base class. 
    /// </summary>
    public class MusicPlayerManager
    {
        /// <summary>
        /// Song source
        /// </summary>
        private IWaveSource _waveSource;
        /// <summary>
        /// Audio output 
        /// </summary>
        private ISoundOut _soundOut;
        /// <summary>
        /// Index of current playing song
        /// </summary>
        private int currentPlayIndex;
        private int volume;

        /// <summary>
        /// Song list
        /// </summary>
        public List<Song> Songs { get; set; }
        /// <summary>
        /// Playing, Stopped, Paused
        /// </summary>
        public PlayerStatus PlayerStatus;
        public string CurrentPlaySong
        {
            get
            {
                if(PlayerStatus != PlayerStatus.Stop)
                    return $"Now playing: {Songs[currentPlayIndex].Title} - {Songs[currentPlayIndex].Artist}";
                return "Stopped";
            }
            private set { } }
        /// <summary>
        /// Volume field
        /// </summary>
        public int Volume
        {
            get
            {
                return (int)_soundOut.Volume * 100;
            }
            set
            {
                if (_soundOut != null)
                    _soundOut.Volume = value / 100f; volume = value;
            }
        }
        /// <summary>
        /// Current part of current playing song
        /// </summary>
        public TimeSpan SongPosition
        {
            get
            {
                if (_waveSource != null)
                    return _waveSource.GetPosition();
                return new TimeSpan(0);
            }
            set
            {
                if (_waveSource != null)
                    _waveSource.SetPosition(value);
            }
        }

        /// <summary>
        /// Song length
        /// </summary>
        public TimeSpan SongLength
        {
            get
            {
                return _waveSource.GetLength();
            }
            private set { }
        }

        public MusicPlayerManager()
        {
            Songs = new List<Song>();
            currentPlayIndex = 0;
            PlayerStatus = PlayerStatus.Stop;
            

            FindSongs(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        /// <summary>
        /// Play selected song. Double clicked song on the list
        /// </summary>
        /// <param name="selectedIndex">Song index on list</param>
        public void PlaySelectedSong(int selectedIndex)
        {
            currentPlayIndex = selectedIndex;
            if (PlayerStatus == PlayerStatus.Play || PlayerStatus == PlayerStatus.Pause)
            {
                _soundOut.Stop();
            }
            PlayASound();
        }

        /// <summary>
        /// Play or pause song
        /// </summary>
        public void Play()
        {
            if (PlayerStatus == PlayerStatus.Play)
            {
                _soundOut.Pause();
                PlayerStatus = PlayerStatus.Pause;
            }
            else
            if (PlayerStatus == PlayerStatus.Pause)
            {
                _soundOut.Play();
                PlayerStatus = PlayerStatus.Play;
            }
            else // if PlayerStatus == PlayerStatus.Stop
            {
                PlayASound();
            }
            
        }

        /// <summary>
        /// Stop playing a song
        /// </summary>
        public void Stop()
        {
            _soundOut.Stop();
            _soundOut.Dispose();
            PlayerStatus = PlayerStatus.Stop;
        }

        /// <summary>
        /// Play previous song on list
        /// </summary>
        public void PlayPrevious()
        {
            if (currentPlayIndex + 1 != 0)
            {
                currentPlayIndex--;
            }

            _soundOut.Stop();
            PlayASound();
        }

        /// <summary>
        /// Play previous song on list
        /// </summary>
        public void PlayNext()
        {
            if (currentPlayIndex + 1 != Songs.Count)
            {
                currentPlayIndex++;
            }

            _soundOut.Stop();
            PlayASound();
        }

        /// <summary>
        /// Close audio output and song source
        /// </summary>
        public void CleanupPlayback()
        {
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        /// <summary>
        /// Find songs in directory
        /// </summary>
        /// <param name="path"></param>
        private void FindSongs(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);

            foreach (var item in files)
            {
                foreach (var extension in Enum.GetValues(typeof(SongsFilesTypes)))
                {
                    var fileExtension = Path.GetExtension(item);
                    if (fileExtension == "." + extension)
                    {
                        Song song = null;
                        if (fileExtension == ".flac")
                        {
                            song = new Song
                            {
                                Title = Path.GetFileNameWithoutExtension(item),
                                Artist = "",
                                Path = item
                            };
                        }
                        else
                        {
                            ID3v2 temp = ID3v2.FromFile(item);
                            var tags = temp.QuickInfo;
                            song = new Song
                            {
                                Title = tags.Title,
                                Artist = tags.LeadPerformers,
                                Track = tags.TrackNumber,
                                Path = item
                            };
                        }
                        if (song != null) Songs.Add(song);
                    }
                }
            }
        }

        /// <summary>
        /// Default function which create audio output and reading song source
        /// </summary>
        private void PlayASound()
        {
            _waveSource = GetSoundSource();
            _soundOut = GetSoundOut();

            _soundOut.Initialize(_waveSource);
            Volume = volume;
            _soundOut.Play();
            PlayerStatus = PlayerStatus.Play;
        }

        /// <summary>
        /// Get audio output
        /// </summary>
        /// <returns>Audio output</returns>
        private ISoundOut GetSoundOut()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                return new WasapiOut();
            else
                return new DirectSoundOut();
        }

        /// <summary>
        /// Get song source
        /// </summary>
        /// <returns>Song source</returns>
        private IWaveSource GetSoundSource()
        {
            return CodecFactory.Instance.GetCodec(Songs[currentPlayIndex].Path);
        }


    }
}
