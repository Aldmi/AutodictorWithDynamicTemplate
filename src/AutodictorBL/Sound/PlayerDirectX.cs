using System;
using System.IO;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using AutodictorBL.Entites;
using AutodictorBL.Settings.XmlSound;
using Domain.Entitys;
using Microsoft.DirectX.AudioVideoPlayback;

namespace AutodictorBL.Sound
{
    public class PlayerDirectX : ISoundPlayer
    {
        #region fields

        private readonly Func<int> _выборУровняГромкостиFunc;
        private readonly Func<string, NotificationLanguage, string> _getFileNameFunc;
        private  string _trackPath = "";
        private  Audio _trackToPlay = null;

        #endregion




        #region prop

        public SoundPlayerType PlayerType { get; }

        private string _statusString;
        public string StatusString
        {
            get { return _statusString; }
            set
            {
                if (value == _statusString) return;
                _statusString = value;
                StatusStringChangeRx.OnNext(_statusString);
            }
        }


        private bool _isConnect;
        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if (value == _isConnect) return;
                _isConnect = value;
                IsConnectChangeRx.OnNext(_isConnect);
            }
        }

        #endregion





        #region ctor

        public PlayerDirectX(SoundPlayerType playerType,Func<int> выборУровняГромкостиFunc, Func<string, NotificationLanguage, string> getFileNameFunc)
        {
            PlayerType = playerType;
            _выборУровняГромкостиFunc = выборУровняГромкостиFunc;
            _getFileNameFunc = getFileNameFunc;
            IsConnect = true;
        }

        #endregion




        #region RxEvent

        public Subject<string> StatusStringChangeRx { get; } = new Subject<string>(); //Изменение StatusString
        public Subject<bool> IsConnectChangeRx { get; } = new Subject<bool>(); //Изменение IsConnect

        #endregion




        #region Methode

        /// <summary>
        /// Получить информацию о плеере
        /// </summary>
        public string GetInfo()
        {
            return "Плеер DirectX, настройки стандартные....";
        }


        public async Task ReConnect()
        {
            IsConnect = true;
        }


        public  bool PlayFile(ВоспроизводимоеСообщение soundMessage)
        {
            var filePath = string.Empty;
            if (soundMessage != null)
            {
                filePath = soundMessage.ИмяВоспроизводимогоФайла;
                var язык = soundMessage.Язык;

                if (filePath.Contains(".wav") == false)
                    filePath = _getFileNameFunc(filePath, язык);
            }

            if (_trackToPlay != null)
            {
                _trackToPlay.Dispose();
                _trackToPlay = null;
            }

            _trackPath = filePath;
            if (_trackPath == "")
                return false;

            try
            {
                if (File.Exists(_trackPath) == true)
                {
                    _trackToPlay = new Audio(_trackPath);
                    SetVolume(_выборУровняГромкостиFunc());
                    _trackToPlay.Play();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public void Pause()
        {
            if (_trackToPlay == null)
                return;
            
            try
            {
                _trackToPlay.Pause();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void Play()
        {
            if (_trackToPlay == null)
                return;

            try
            {
                _trackToPlay.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public float GetDuration()
        {
            try
            {
                if (_trackToPlay != null)
                    return (float)_trackToPlay.Duration;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0f;
            };

            return 0f;
        }


        public int GetCurrentPosition()
        {
            try
            {
                if (_trackToPlay != null)
                    return (int)_trackToPlay.CurrentPosition;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            };

            return 0;
        }



        //TODO: Exceptions при геннерации списка.
        public  SoundPlayerStatus GetPlayerStatus()
        {
            SoundPlayerStatus playerStatus = SoundPlayerStatus.Error;

            try
            {
                if (_trackToPlay != null)
                {
                    if (_trackToPlay.Playing)
                    {
                        if (_trackToPlay.CurrentPosition >= _trackToPlay.Duration)
                            return SoundPlayerStatus.Stop;

                        return SoundPlayerStatus.Playing;
                    }
                    else if (_trackToPlay.Paused)
                        return SoundPlayerStatus.Paused;
                    else
                        return SoundPlayerStatus.Stop;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };

            return playerStatus;
        }


        public  int GetVolume()
        {
            if (_trackToPlay != null)
                return _trackToPlay.Volume;

            return 0;
        }


        public void SetVolume(int volume)
        {
            if (_trackToPlay != null)
            {
                _trackToPlay.Volume = volume;
            }
        }

        #endregion





        #region Dispouse

        public void Dispose()
        {
            if (_trackToPlay != null && !_trackToPlay.Disposed)
            {
                _trackToPlay.Dispose();
            }
        }

       #endregion
    }
}
