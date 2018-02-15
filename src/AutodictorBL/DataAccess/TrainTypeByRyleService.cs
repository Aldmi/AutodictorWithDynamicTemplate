using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace AutodictorBL.DataAccess
{
    public class TrainTypeByRyleService
    {
        private readonly ITrainTypeByRyleRepository _repository;



        #region ctor

        public TrainTypeByRyleService(ITrainTypeByRyleRepository repository)
        {
            _repository = repository;
        }

        #endregion




        #region Methode

        public TrainTypeByRyle GetById(int id)
        {
            return _repository.GetById(id);
        }


        public IEnumerable<TrainTypeByRyle> GetAll()
        {
            return _repository.List();
        }

        public int GetIndexOf(TrainTypeByRyle rule)
        {
            if (rule == null)
            {
                return -1;
            }

            var findItem = GetAll().FirstOrDefault(r=>r.Id == rule.Id);
            return GetAll().ToList().IndexOf(findItem);
        }

        #endregion
    }
}