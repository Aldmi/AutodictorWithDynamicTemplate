using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace AutodictorBL.DataAccess
{
    public class PathwaysService
    {
        private readonly IPathwaysRepository _repository;



        #region ctor

        public PathwaysService(IPathwaysRepository repository)
        {
            _repository = repository;
        }

        #endregion




        #region Methode

        public Pathway GetById(int id)
        {
            return _repository.GetById(id);
        }


        public IEnumerable<Pathway> GetAll()
        {
            return _repository.List();
        }

        public IEnumerable<Pathway> GetAllByFilter(Expression<Func<Pathway, bool>> predicate)
        {
            return _repository.List(predicate);
        }

        #endregion
    }
}