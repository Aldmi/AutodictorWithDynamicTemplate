using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;


namespace DAL.Serialize.XML.Reposirory
{
    public class XmlSerializeTableRecRepository : ITrainTableRecRepository
    {
        private List<TrainTableRec> ListMoq { get; set; }


        #region ctor

        public XmlSerializeTableRecRepository(string connection)
        {
            ListMoq= new List<TrainTableRec>
            {
                new TrainTableRec
                {
                    Id= 1,
                    Active = true,
                    ArrivalTime = new DateTime(2018, 02, 2, 10,12,0),
                    DepartureTime = new DateTime(2018, 02, 2, 10,20,0),
                    Num = "25"
                },
                new TrainTableRec
                {
                    Id= 2,
                    Active = true,
                    ArrivalTime = new DateTime(2018, 02, 2, 10,12,0),
                    DepartureTime = new DateTime(2018, 02, 2, 10,20,0),
                    Num = "65"
                }
            };
        }

        #endregion




        public TrainTableRec GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrainTableRec> List()
        {
            return ListMoq;
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