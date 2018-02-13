using System.Collections.Generic;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace AutodictorBL.DataAccess
{
    public class DirectionService
    {
        private readonly IDirectionRepository _repository;



        #region ctor

        public DirectionService(IDirectionRepository repository)
        {
            _repository = repository;
        }

        #endregion




        #region Methode

        public Direction GetById(int id)
        {
            return _repository.GetById(id);
        }


        public IEnumerable<Direction> GetAll()
        {
            return _repository.List();
        }

        #endregion
    }
}