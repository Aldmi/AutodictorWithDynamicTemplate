using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Xml.Serialization;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
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
                var listTrainTableRecXmlModel = XmlSerializerWorker.Load<List<TrainTableRecXmlModel>>(_folderName, _fileName);
                //listTrainTableRecXmlModel --> listTrainTableRec
            }
            catch (Exception)
            {
                return null;
            }

            return new List<TrainTableRec>();
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
            var debbugingList = new List<TrainTableRecXmlModel> //DEBUG
            {
                new TrainTableRecXmlModel
                {
                    Id = 100,
                    Name = "Саратов-Ростов",
                    Num = "456",
                    Num2 = null,
                    Примечание = "l;l;l;"
                },
                new TrainTableRecXmlModel
                {
                    Id = 2,
                    Name = "Адлер-Анапа",
                    Num = null,
                    Num2 = "dsd",
                    Примечание = null
                },
            };

            //XmlSerializerWorker.Save(debbugingList, _folderName, _fileName);

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