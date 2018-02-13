using System;
using System.Collections.Generic;
using Domain.Abstract;
using Domain.Entitys;

namespace AutodictorBL.DataAccess
{
    public class AccessTrainTypeByRyle
    {
        private readonly IRepository<TrainTypeByRyle> _repository;



        #region ctor

        public AccessTrainTypeByRyle(IRepository<TrainTypeByRyle> repository)
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