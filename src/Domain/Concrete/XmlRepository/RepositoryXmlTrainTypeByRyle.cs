using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Domain.Abstract;
using Domain.Entitys;
using Library.Xml;

namespace Domain.Concrete.XmlRepository
{
    public class RepositoryXmlTrainTypeByRyle : IRepository<TrainTypeByRyle>
    {
        private readonly XElement _xElement;
        private IEnumerable<TrainTypeByRyle> TrainTypeByRyles { get; set; }




        #region ctor

        public RepositoryXmlTrainTypeByRyle(XElement xElement)
        {
            if (xElement == null)
                throw new ArgumentNullException(nameof(xElement));
            _xElement = xElement;
        }

        public RepositoryXmlTrainTypeByRyle()
        {        
            var xmlFile = XmlWorker.LoadXmlFile("Config", "DynamicSound.xml"); //все настройки в одном файле
            if (xmlFile == null)
                throw new FileNotFoundException("Файл не найден Config/DynamicSound.xml");
            _xElement = xmlFile;
        }

        #endregion





        public TrainTypeByRyle GetById(int id)
        {
            throw new NotImplementedException();
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
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<TrainTypeByRyle> List(Expression<Func<TrainTypeByRyle, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public void Add(TrainTypeByRyle entity)
        {
            throw new NotImplementedException();
        }


        public void AddRange(IEnumerable<TrainTypeByRyle> entity)
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
    }
}