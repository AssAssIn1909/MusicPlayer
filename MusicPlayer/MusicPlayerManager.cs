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
    public class MusicPlayerManager
    {
        private IWaveSource _waveSource;
        private ISoundOut _soundOut;
        private int currentPlay;
        private int volume;


        public List<Song> Songs { get; set; }
        public PlayerStatus PlayerStatus;
        public int Volume { get { return (int)_soundOut.Volume * 100; } set { if (_soundOut != null) _soundOut.Volume = value / 100f; volume = value; } }
        public TimeSpan Position
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


        public TimeSpan Length { get { return _waveSource.GetLength(); } private set { } }

        public MusicPlayerManager()
        {
            Songs = new List<Song>();
            currentPlay = 0;
            PlayerStatus = PlayerStatus.Stop;
            

            FindSongs(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }


        public void PlaySelectedSong(int selectedIndex)
        {
            currentPlay = selectedIndex;
            if (PlayerStatus == PlayerStatus.Play || PlayerStatus == PlayerStatus.Pause)
            {
                _soundOut.Stop();
            }
            PlayASound();
        }

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
            else
            {
                PlayASound();
            }
            
        }

        public void Stop()
        {
            _soundOut.Stop();
            _soundOut.Dispose();
            PlayerStatus = PlayerStatus.Stop;
        }

        public void PlayPrevious()
        {
            if (currentPlay + 1 != 0)
            {
                currentPlay--;
            }

            _soundOut.Stop();
            PlayASound();
        }

        public void PlayNext()
        {
            if (currentPlay + 1 != Songs.Count)
            {
                currentPlay++;
            }

            _soundOut.Stop();
            PlayASound();
        }

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

        private void FindSongs(string path)
        {
            var files = Directory.GetFiles(path);

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
                                Author = "",
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
                                Author = tags.Artist,
                                Track = tags.TrackNumber,
                                Path = item
                            };
                        }
                        if (song != null) Songs.Add(song);
                    }
                }
            }
        }

        private void PlayASound()
        {
            _waveSource = GetSoundSource();
            _soundOut = GetSoundOut();

            _soundOut.Initialize(_waveSource);
            Volume = volume;
            _soundOut.Play();
            PlayerStatus = PlayerStatus.Play;
        }

        private ISoundOut GetSoundOut()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                return new WasapiOut();
            else
                return new DirectSoundOut();
        }

        private IWaveSource GetSoundSource()
        {
            return CodecFactory.Instance.GetCodec(Songs[currentPlay].Path);
        }


    }
}
