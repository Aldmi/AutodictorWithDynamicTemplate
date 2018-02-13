using System;
using System.Collections.Generic;
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

        #endregion
    }
}