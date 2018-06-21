﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using Library.Xml;


namespace DAL.XmlRaw.Repository
{
    public class XmlRawTrainTypeByRuleRepository : ITrainTypeByRyleRepository
    {
        private readonly XElement _xElement;
        private IEnumerable<TrainTypeByRyle> TrainTypeByRyles { get; set; }




        #region ctor

        public XmlRawTrainTypeByRuleRepository(XElement xElement)
        {
            if (xElement == null)
                throw new ArgumentNullException(nameof(xElement));
            _xElement = xElement;
        }

        public XmlRawTrainTypeByRuleRepository(string folderName, string fileName)
        {
            var xmlFile = FileWorker.LoadXmlFile(folderName, fileName);
            if (xmlFile == null)
                throw new FileNotFoundException("Файл не найден Config/DynamicSound.xml");
            _xElement = xmlFile;
        }

        #endregion




        #region Methode

        public TrainTypeByRyle GetById(int id)
        {
            return List().FirstOrDefault(rule => rule.Id == id);
        }


        public IEnumerable<TrainTypeByRyle> List()
        {
            return TrainTypeByRyles ?? (TrainTypeByRyles = ParseXmlFile());
        }


        private IEnumerable<TrainTypeByRyle> ParseXmlFile()
        {
            var rules = new List<TrainTypeByRyle>();
            try
            {
                var trainTypes = _xElement?.Elements("TrainType").ToList();
                if (trainTypes == null || !trainTypes.Any())
                    return null;

                foreach (var el in trainTypes)
                {
                    //ПАРСИНГ ШАБЛОНОВ--------------------------------
                    var listTemplateActs = new List<ActionTrain>();
                    var templateActions = el.Element("TemplateActions");
                    if (templateActions != null)
                    {
                        var actions = templateActions.Elements("Action").ToList();
                        foreach (var act in actions)
                        {
                            var langs = act.Elements("Lang").ToList();
                            var listLangs = new List<Lang>();
                            foreach (var lang in langs)
                            {
                                listLangs.Add(new Lang(
                                    (string)lang.Attribute("Id"),
                                    (string)lang.Attribute("Name"),
                                    (string)lang.Attribute("RepeatSoundBody"),
                                    (string)lang.Attribute("SoundStart"),
                                    (string)lang.Attribute("SoundBody"),
                                    (string)lang.Attribute("SoundEnd")));
                            }

                            //Добавим ActionTrain
                            var times = (string)act.Attribute("Time");
                            if (!string.IsNullOrEmpty(times))
                            {
                                listTemplateActs.Add(new ActionTrain(
                                    (string)act.Attribute("Id"),
                                    (string)act.Attribute("Enable"),
                                    (string)act.Attribute("Name"),
                                    (string)act.Attribute("Type"),
                                    (string)act.Attribute("Priority"),
                                    (string)act.Attribute("Transit"),
                                    (string)act.Attribute("Emergency"),
                                    (string)act.Attribute("Time"),
                                    listLangs));
                            }
                        }
                    }

                    //ПАРСИНГ НЕШТАТОК--------------------------------
                    var listEmergencyActs = new List<ActionTrain>();
                    var emergencyActions = el.Element("EmergencyActions");
                    if (emergencyActions != null)
                    {
                        var actions = emergencyActions.Elements("Action").ToList();
                        foreach (var act in actions)
                        {
                            var langs = act.Elements("Lang").ToList();
                            var listLangs = new List<Lang>();
                            foreach (var lang in langs)
                            {
                                listLangs.Add(new Lang(
                                    (string)lang.Attribute("Id"),
                                    (string)lang.Attribute("Name"),
                                    (string)lang.Attribute("RepeatSoundBody"),
                                    (string)lang.Attribute("SoundStart"),
                                    (string)lang.Attribute("SoundBody"),
                                    (string)lang.Attribute("SoundEnd")));
                            }

                            //Добавим ActionTrain
                            var times = (string)act.Attribute("Time");
                            if (!string.IsNullOrEmpty(times))
                            {
                                listEmergencyActs.Add(new ActionTrain(
                                    (string)act.Attribute("Id"),
                                    true.ToString(),
                                    (string)act.Attribute("Name"),
                                    (string)act.Attribute("Type"), 
                                    (string)act.Attribute("Priority"),
                                    false.ToString(),
                                    (string)act.Attribute("Emergency"),
                                    (string)act.Attribute("Time"),
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
                        listTemplateActs,
                        listEmergencyActs));
                }

                return rules;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<TrainTypeByRyle> List(Expression<Func<TrainTypeByRyle, bool>> predicate)
        {
            return List().Where(predicate.Compile());
        }


        public void Add(TrainTypeByRyle entity)
        {
            throw new NotImplementedException();
        }


        public void AddRange(IEnumerable<TrainTypeByRyle> entitys)
        {
            throw new NotImplementedException();
        }


        public void Delete(TrainTypeByRyle entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Expression<Func<TrainTypeByRyle, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public void Edit(TrainTypeByRyle entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}