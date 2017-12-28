using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AutodictorBL.Settings.XmlSound;
using Domain.Entitys;

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




        /// <summary>
        /// Создание списка настроек для правил поезда зависмых от ТИПА поезда
        /// </summary>
        public static List<TrainTypeByRyle> CreateXmlTrainTypeRules(XElement xml)
        {
            var trainTypes = xml?.Elements("TrainType").ToList();
            if (trainTypes == null || !trainTypes.Any())
                return null;

            var rules = new List<TrainTypeByRyle>();
            foreach (var el in trainTypes)
            {
                var actions = el.Elements("Action").ToList();
                var listActs = new List<ActionTrain>();
                foreach (var act in actions)
                {
                    var langs = act.Elements("Lang").ToList();
                    var listLangs = new List<Lang>();
                    foreach (var lang in langs)
                    {
                        listLangs.Add(new Lang(
                            (string)lang.Attribute("Id"),
                            (string)lang.Attribute("Name"),
                            (string)lang.Attribute("SoundStart"),
                            (string)lang.Attribute("SoundBody"),
                            (string)lang.Attribute("SoundEnd")));
                    }

                    //для каждого "Time" указанного через зяпятую, создается копия шаблона (т.е. у кахждого шаблона одно время, циклическое или обычное)
                    var times = (string)act.Attribute("Time");
                    if (!string.IsNullOrEmpty(times))
                    {
                        var deltaTimes = times.Split(',').ToList();
                        foreach (var deltaTime in deltaTimes)
                        {
                            listActs.Add(new ActionTrain(
                                (string)act.Attribute("Id"),
                                (string)act.Attribute("Name"),
                                (string)act.Attribute("Type"),
                                (string)act.Attribute("Priority"),
                                (string)act.Attribute("Repeat"),
                                (string)act.Attribute("Transit"),
                                (string)act.Attribute("Emergency"),
                                deltaTime,
                                listLangs));
                        }
                    }
                }

                rules.Add(new TrainTypeByRyle(
                    (string)el.Attribute("Id"),
                    (string)el.Attribute("Type"),
                    (string)el.Attribute("NameRu"),
                    (string)el.Attribute("AliasRu"),
                    (string)el.Attribute("NameEng"),
                    (string)el.Attribute("AliasEng"),
                    (string)el.Attribute("NameCh"),
                    (string)el.Attribute("AliasCh"),
                    (string)el.Attribute("ShowPathTimer"),
                    (string)el.Attribute("WarningTimer"),
                    listActs));
            }

            return rules;
        }

    }
}