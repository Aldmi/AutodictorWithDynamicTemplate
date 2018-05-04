using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using AutoMapper;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DAL.Serialize.Mappers;
using DAL.Serialize.XML.Model;
using Library.Xml;


namespace DAL.Serialize.XML.Reposirory
{
    public class XmlSerializeTrainTableRecRepository : ITrainTableRecRepository
    {
        #region field

        private readonly string _key;
        private readonly string _folderName;
        private readonly string _fileName;

        #endregion



        #region ctor

        public XmlSerializeTrainTableRecRepository(string key, string folderName, string fileName)
        {
            _key = key;
            _folderName = folderName;
            _fileName = fileName;

            AutoMapperConfig.Register();
        }

        #endregion




        #region Methode CRUD

        public TrainTableRec GetById(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<TrainTableRec> List() //TODO: realize
        {
            try
            {
                var listTrainTableRecXmlModel = XmlSerializerWorker.Load<ListTrainRecsXml>(_folderName, _fileName).TrainTableRecXmlModels;


                var first = listTrainTableRecXmlModel.FirstOrDefault();
                var firstMap= Mapper.Map<TrainTableRec>(first);

                var listTrainRecs = new TrainTableRec[10]; // Mapper.Map<IEnumerable<TrainTableRec>>(listTrainTableRecXmlModel).ToList();
                return listTrainRecs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public IEnumerable<TrainTableRec> List(Expression<Func<TrainTableRec, bool>> predicate) //TODO: realize
        {
            throw new NotImplementedException();
        }


        public void Add(TrainTableRec entity)
        {
            throw new NotImplementedException();
        }


        public void AddRange(IEnumerable<TrainTableRec> entitys)//TODO: realize
        {
           var first= entitys.FirstOrDefault();
           first.Route= new Route {RouteType = RouteType.WithStopsAt, Stations = new List<Station> {new Station {Id = 10} } };
           first.ИспользоватьДополнение["звук"] = true;

           var listTrainRecsXml = Mapper.Map<IEnumerable<TrainTableRecXmlModel>>(entitys).ToList();

            //var debbugingList = new List<TrainTableRecXmlModel> //DEBUG
            //{
            //    new TrainTableRecXmlModel
            //    {
            //        Id = 100,
            //        Name = "Саратов-Ростов",
            //        Num = "456",
            //        Num2 = null,
            //        Примечание = "l;l;l;",
            //       // RouteXmlModel = new RouteXmlModel {RouteType = RouteType.WithStopsAt, StationsId = new List<int> {2,5,6,78 }},
            //       // ArrivalTime = DateTime.Now,
            //       // StopTime = new TimeSpan(10,2,3),
            //       //// TrainPathNumber = new List<KeyValuePair<WeekDays, int>> {new KeyValuePair<WeekDays, int>(WeekDays.Постоянно, 10) }
            //       //TrainPathNumber = new XmlSerializableDictionary<WeekDays, int>
            //       //{
            //       //    {WeekDays.Постоянно, 10 }
            //       //}
                    
            //    },
            //    new TrainTableRecXmlModel
            //    {
            //        Id = 2,
            //        Name = "Адлер-Анапа",
            //        Num = null,
            //        Num2 = "dsd",
            //        Примечание = null,
            //        //ArrivalTime = null,
            //        //StopTime = new TimeSpan(10,2,3)
            //    },
            //};

            var container = new ListTrainRecsXml { TrainTableRecXmlModels = listTrainRecsXml };
            XmlSerializerWorker.Save(container, _folderName, _fileName);
        }


        public void Delete(TrainTableRec entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<TrainTableRec, bool>> predicate) //TODO: realize
        {
           
        }

        public void Edit(TrainTableRec entity)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}