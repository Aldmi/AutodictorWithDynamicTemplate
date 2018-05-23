using System;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutodictorBL.Models;
using AutodictorBL.Settings.XmlSound;
using AutodictorBL.Sound.Converters;
using PRAESIDEOOPENINTERFACECOMSERVERLib;


namespace AutodictorBL.Sound
{
    public class PlayerOmneo : ISoundPlayer
    {
        #region Fileld

        private readonly PraesideoOpenInterface_V0430 _praesideoOi;
        private int? _currentCallId;
        private readonly string _ip;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _defaultZoneNames;          //Название маршрутов по умолчанию (через запятую). Если в ВоспроизводимоеСообщение не указанн маршрут, пользуемя маршрутом по умолчанию
        private readonly int _timeDelayReconnect;
        private readonly int _timeResponse;                 //Время контроллера  на ответ о начале проигрывания файла.
        private SoundPlayerStatus _soundPlayerStatus;

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

        public IFileNameConverter FileNameConverter => new Omneo8CharacterFileNameConverter();

        #endregion




        #region ctor

        public PlayerOmneo(SoundPlayerType playerType, string ip, int port, string userName, string password, string defaultZoneNames, int timeDelayReconnect, int timeResponse)
        {
            _ip = ip;
            _port = port;
            _userName = userName;
            _password = password;
            _defaultZoneNames = defaultZoneNames;
            _timeDelayReconnect = timeDelayReconnect;
            _timeResponse = timeResponse;
            PlayerType = playerType;

            _praesideoOi = new PraesideoOpenInterface_V0430();
            _praesideoOi.connectionBroken += PraesideoOiOnConnectionBroken;
            _praesideoOi.callState += PraesideoOI_callState;
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
            var getAudioInputNames = _praesideoOi.getAudioInputNames() ?? "NULL";
            var getBgmChannelNames = _praesideoOi.getBgmChannelNames() ?? "NULL";
            var getChimeNames = _praesideoOi.getChimeNames() ?? "NULL";
            var getConfigId = _praesideoOi.getConfigId();
            var getConfiguredUnits = _praesideoOi.getConfiguredUnits() ?? "NULL";
            var getConnectedUnits = _praesideoOi.getConnectedUnits() ?? "NULL";
            var getMessageNames = _praesideoOi.getMessageNames() ?? "NULL";
            var getNcoVersion = _praesideoOi.getNcoVersion() ?? "NULL";
            var getVersion = _praesideoOi.getVersion() ?? "NULL";
            var getVirtualControlInputNames = _praesideoOi.getVirtualControlInputNames() ?? "NULL";
            var getZoneGroupNames = _praesideoOi.getZoneGroupNames() ?? "NULL";          //Зоны объединяются в группы, имена групп зон и есть маршрут. (пустая строка для всех зон)


            return $@"getAudioInputNames = {getAudioInputNames}" + "\n" +
                   $"getBgmChannelNames = {getBgmChannelNames}" + "\n" +
                   $"getChimeNames = {getChimeNames}" + "\n" +                //имя файла стартового звонка (перед основным сообщением)
                   $"getConfigId = {getConfigId}" + "\n" +
                   $"getConfiguredUnits = {getConfiguredUnits}" + "\n" +
                   $"getConnectedUnits = {getConnectedUnits}" + "\n" +
                   $"getMessageNames = {getMessageNames}" + "\n" +
                   $"getNcoVersion = {getNcoVersion}" + "\n" +
                   $"getVersion = {getVersion}" + "\n" +
                   $"getVirtualControlInputNames = {getVirtualControlInputNames}" + "\n" +
                   $"getZoneGroupNames = {getZoneGroupNames}";
        }


        public async Task ReConnect()
        {
            Disconnect();
            await Connect();
        }


        private async Task Connect()
        {
            while (!IsConnect)
            {
                try
                {
                    await Task.Factory.StartNew(() =>
                    {
                        _praesideoOi.connect(_ip, _port, _userName, _password);
                    });
                    IsConnect = true;
                }

                catch (COMException ex)
                {
                    Disconnect();
                    IsConnect = false;
                    StatusString = $"Exception Ошибка подключения: \"{ex.Message}\"  \"{ex.InnerException?.Message}\"";
                }
                catch (Exception ex)
                {
                    Disconnect();
                    IsConnect = false;
                    StatusString = $"Exception НЕИЗВЕСТНАЯ Ошибка подключения: \"{ex.Message}\"  \"{ex.InnerException?.Message}\"";
                    //Log.log.Fatal($"ERROR  (ConnectHttp)   Message= {StatusString}"); //DEBUG_LOG
                }
                finally
                {
                    await Task.Delay(_timeDelayReconnect);
                }
            }
        }



        public void Disconnect()
        {
            _praesideoOi.disconnect();
            IsConnect = false;
        }



        public bool PlayFile(ВоспроизводимоеСообщение soundMessage, bool useFileNameConverter = true)
        {
            if (!IsConnect)
                return false;

            try
            {
                int priority = 100;
                bool bPartial = false;
                string startChime = string.Empty;
                string endChime = string.Empty;
                bool bLiveSpeech = false;
                string audioInput = string.Empty;
                string messages = useFileNameConverter ? FileNameConverter.Convert(soundMessage.ИмяВоспроизводимогоФайла) : soundMessage.ИмяВоспроизводимогоФайла;
                int repeat = 1;
                _currentCallId = _praesideoOi.createCall(_defaultZoneNames, priority, bPartial, startChime, endChime, bLiveSpeech, audioInput, messages, repeat);

                Play();
                return true;
            }
            catch (Exception ex)
            {
                StatusString = @"Exception PlayFile: " + ex.Message;
                return false;
            }
        }
        

        /// <summary>
        /// Запускает задачу _tcs которая длится _timeResponse (мсек)  - допустимое время на оповещение.
        /// Усилитель оповестит о реальном запуске файла для проигрывания в обработчике события PraesideoOI_callState.
        /// Это завершит задачу, раньше чем пройдет 3 сек или возникнет Exception внутри PlayWithControl.
        /// </summary>
        private TaskCompletionSource<bool> _tcs;
        private Task<bool> PlayWithControl()
        {
            _tcs = new TaskCompletionSource<bool>();
            Task.Run(async () =>
            {
                if (_currentCallId != null)
                {
                    try
                    {
                        _praesideoOi.startCreatedCall(_currentCallId.Value);
                        await Task.Delay(_timeResponse);
                        _tcs.TrySetResult(false);
                    }
                    catch (Exception)
                    {
                        _tcs.TrySetResult(false);
                    }
                }
            });

            return _tcs.Task;
        }



        public async void Play()
        {
            if (!IsConnect)
                return;

            var resul = await PlayWithControl();
            if (resul == false)
            {
                await ReConnect();
            }
        }



        public void Pause()
        {
            if (!IsConnect)
                return;

            if (_currentCallId != null)
            {
                try
                {
                    _praesideoOi.stopCall(_currentCallId.Value);
                }
                catch (Exception)
                {

                }
            }
        }



        public float GetDuration()
        {
            if (!IsConnect)
                return 0;

            throw new NotImplementedException();
        }



        public int GetCurrentPosition()
        {
            if (!IsConnect)
                return 0;

            throw new NotImplementedException();
        }



        public SoundPlayerStatus GetPlayerStatus()
        {
            if (!IsConnect)
                return SoundPlayerStatus.Error;

            return _soundPlayerStatus;
        }



        public int GetVolume()
        {
            if (!IsConnect)
                return 0;

            throw new NotImplementedException();
        }



        public void SetVolume(int volume)
        {
            if (!IsConnect)
                return;

            throw new NotImplementedException();
        }

        #endregion





        #region EventHandler

        /// <summary>
        /// вызывается при разрыве соединения
        /// </summary>
        private async void PraesideoOiOnConnectionBroken()
        {
            await ReConnect();
        }


        /// <summary>
        /// вызывается при изменении состояния вызова
        /// </summary>
        private void PraesideoOI_callState(int callId, [System.Runtime.InteropServices.ComAliasName("PRAESIDEOOPENINTERFACECOMSERVERLib.TOICallState")] TOICallState state)
        {
            switch (state)
            {
                case TOICallState.OICS_START:
                    _tcs.TrySetResult(true);
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    StatusString = "СТАРТ проигрывания";
                    break;

                case TOICallState.OICS_STARTCHIME:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    break;

                case TOICallState.OICS_MESSAGES:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    break;

                case TOICallState.OICS_LIVESPEECH:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    break;

                case TOICallState.OICS_END:
                    _soundPlayerStatus = SoundPlayerStatus.Idle;
                    StatusString = "СТОП проигрывания";
                    break;

                case TOICallState.OICS_ABORT:
                    _tcs.TrySetResult(true);
                    _soundPlayerStatus = SoundPlayerStatus.Stop;
                    break;

                case TOICallState.OICS_IDLE:
                    _soundPlayerStatus = SoundPlayerStatus.Idle;
                    break;

                case TOICallState.OICS_REPLAY:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    break;
            }
        }

        #endregion





        #region Dispouse

        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}