using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AutodictorBL.Settings.XmlSound;


namespace AutodictorBL.Settings
{
    public class XmlSettingFactory
    {
        /// <summary>
        /// Создание списка настроек для устройств подключенных по Http
        /// </summary>
        public static List<XmlSoundPlayerSettings> CreateXmlSoundPlayerSettings(XElement xml)
        {
            var soundPlayers = xml?.Element("Sound")?.Elements("Player").ToList();
            var players = new List<XmlSoundPlayerSettings>();

            if (soundPlayers == null || !soundPlayers.Any())
                return players;

            foreach (var el in soundPlayers)
            {
                var playerType = (SoundPlayerType)Enum.Parse(typeof(SoundPlayerType), (string)el.Attribute("Type"));
                switch (playerType)
                {
                    case SoundPlayerType.DirectX:
                        var playerSett = new XmlSoundPlayerSettings(playerType);
                        players.Add(playerSett);
                        break;

                    case SoundPlayerType.Omneo:
                        playerSett = new XmlSoundPlayerSettings(playerType,
                            (string)el.Attribute("Ip"),
                            (string)el.Attribute("Port"),
                            (string)el.Attribute("UserName"),
                            (string)el.Attribute("Password"),
                            (string)el.Attribute("DefaultZoneNames"),
                            (string)el.Attribute("TimeDelayReconnect"),
                            (string)el.Attribute("TimeResponse"));
                        players.Add(playerSett);
                        break;
                }
            }
            return players;
        }


    }
}