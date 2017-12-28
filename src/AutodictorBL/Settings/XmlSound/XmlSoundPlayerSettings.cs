using System;
using System.Xml.Linq;

namespace AutodictorBL.Settings.XmlSound
{
    public enum SoundPlayerType
    {
        None, DirectX, Omneo
    }


    public class XmlSoundPlayerSettings
    {
        #region prop

        public SoundPlayerType PlayerType { get; set; }

        public string Ip { get; }
        public int Port { get; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DefaultZoneNames { get; set; }
        public int TimeDelayReconnect { get; set; }
        public int TimeResponse { get; set; }

        #endregion




        #region ctor

        public XmlSoundPlayerSettings(SoundPlayerType type)
        {
            PlayerType = type;
        }

        public XmlSoundPlayerSettings(SoundPlayerType type, string ip, string port, string userName, string password, string defaultZoneNames, string timeDelayReconnect, string timeResponse)
        {
            PlayerType = type;

            Ip = ip;
            Port = int.Parse(port);
            UserName = userName;
            Password = password;
            DefaultZoneNames = defaultZoneNames;
            TimeDelayReconnect = int.Parse(timeDelayReconnect);
            TimeResponse = int.Parse(timeResponse);
        }

        #endregion

    }
}