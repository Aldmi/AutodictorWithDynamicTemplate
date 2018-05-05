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
        }


        static XmlSerializeTrainTableRecRepository()
        {
            AutoMapperConfig.Register();
        }

        #endregion




        #region Methode CRUD

        public TrainTableRec GetById(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<TrainTableRec> List()
        {
            try
            {
                var listTrainTableRecXmlModel = XmlSerializerWorker.Load<ListTrainRecsXml>(_folderName, _fileName).TrainTableRecXmlModels;
                var listTrainTableRec = Mapper.Map<IList<TrainTableRec>>(listTrainTableRecXmlModel);
                return listTrainTableRec;
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
            var listTrainRecsXml = Mapper.Map<IEnumerable<TrainTableRecXmlModel>>(entitys).ToList();
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