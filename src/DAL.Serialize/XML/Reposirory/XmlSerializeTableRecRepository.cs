using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;


namespace DAL.Serialize.XML.Reposirory
{
    public class XmlSerializeTableRecRepository : ITrainTableRecRepository
    {


        #region ctor

        public XmlSerializeTableRecRepository(string connection)
        {
            
        }

        #endregion




        public TrainTableRec GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrainTableRec> List()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrainTableRec> List(Expression<Func<TrainTableRec, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(TrainTableRec entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TrainTableRec> entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TrainTableRec entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<TrainTableRec, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Edit(TrainTableRec entity)
        {
            throw new NotImplementedException();
        }
    }
}