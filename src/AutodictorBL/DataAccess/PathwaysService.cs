using System.Collections.Generic;
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

        public Pathways GetById(int id)
        {
            return _repository.GetById(id);
        }


        public IEnumerable<Pathways> GetAll()
        {
            return _repository.List();
        }

        #endregion
    }
}