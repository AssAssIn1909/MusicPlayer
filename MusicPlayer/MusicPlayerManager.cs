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
        private PlayerStatus playerStatus;
        private int currentPlay;

        public List<Song> Songs { get; set; }

        public MusicPlayerManager()
        {
            Songs = new List<Song>();
            currentPlay = 0;
            playerStatus = PlayerStatus.Stop;


            FindSongs(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            PlayASound();
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

        internal void PlaySelectedSong(int selectedIndex)
        {
            currentPlay = selectedIndex;
            PlayASound();
        }

        public void Play()
        {
            if (playerStatus == PlayerStatus.Play)
            {
                _soundOut.Pause();
                playerStatus = PlayerStatus.Pause;
            }
            else
            if (playerStatus == PlayerStatus.Pause)
            {
                _soundOut.Play();
                playerStatus = PlayerStatus.Play;
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
            playerStatus = PlayerStatus.Stop;
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

        private void PlayASound()
        {
            _waveSource = GetSoundSource();
            _soundOut = GetSoundOut();

            _soundOut.Initialize(_waveSource);
            _soundOut.Play();
            playerStatus = PlayerStatus.Play;
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
            //return any source ... in this example, we'll just play a mp3 file
            return CodecFactory.Instance.GetCodec(Songs[currentPlay].Path);
        }


        private void CleanupPlayback()
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

        public void Dispose()
        {
            CleanupPlayback();
        }
    }
}
