using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutodictorBL.Settings;
using AutodictorBL.Settings.XmlSound;
using AutodictorBL.Sound;
using Domain.Entitys;
using Library.Logs;
using Library.Xml;


namespace AutodictorBL
{
    public class AutodictorModel: IDisposable
    {
        #region prop

        public List<Task> BackGroundTasks { get; set; } = new List<Task>();
        public ISoundPlayer SoundPlayer { get; set; }

        private string _errorString;
        public string ErrorString
        {
            get { return _errorString; }
            set
            {
                if (value == _errorString) return;
                _errorString = value;
                //сработка события
            }
        }

        #endregion






        //TODO: Настройки из Program перенести в AutodictorModel
        public void LoadSetting(Func<int> выборУровняГромкостиFunc, Func<string, NotificationLanguage, string> getFileNameFunc)
        {
            //ЗАГРУЗКА НАСТРОЕК----------------------------------------------------------------------------------------------------------------------------
            List<XmlSoundPlayerSettings> xmlSoundPlayers;

            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Settings", "Setting.xml"); //все настройки в одном файле
                if (xmlFile == null)
                    return;

                xmlSoundPlayers = XmlSettingFactory.CreateXmlSoundPlayerSettings(xmlFile);
            }
            catch (FileNotFoundException ex)
            {
                ErrorString = "Файл Setting.xml не найденн";
                //Log.log.Error(ErrorString);
                return;
            }
            catch (Exception ex)
            {
                ErrorString = "ОШИБКА в узлах дерева XML файла настроек:  " + ex;
                Console.WriteLine(ex.Message);
                //Log.log.Error(ErrorString);
                return;
            }


            //СОЗДАНИЕ SoundPlayer-----------------------------------------------------------------------
            if (xmlSoundPlayers != null && xmlSoundPlayers.Any())
            {
                var firstXmlPlayer = xmlSoundPlayers.FirstOrDefault();
                switch (firstXmlPlayer.PlayerType)
                {
                    case SoundPlayerType.DirectX:
                        SoundPlayer = new PlayerDirectX(firstXmlPlayer.PlayerType, выборУровняГромкостиFunc, getFileNameFunc);
                        break;

                    case SoundPlayerType.Omneo:
                        var player = new PlayerOmneo(firstXmlPlayer.PlayerType, firstXmlPlayer.Ip, firstXmlPlayer.Port, firstXmlPlayer.UserName, firstXmlPlayer.Password, firstXmlPlayer.DefaultZoneNames, firstXmlPlayer.TimeDelayReconnect, firstXmlPlayer.TimeResponse);
                        var task = player.ReConnect();   //выполняется фоновая задача, пока не подключится к контроллеру усилителя.
                        BackGroundTasks?.Add(task);
                        SoundPlayer = player;
                        break;
                }
            }

        }



        #region Dispouse

        public void Dispose()
        {
            SoundPlayer?.Dispose();
        }

        #endregion
    }
}