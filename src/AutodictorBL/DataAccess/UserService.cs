using System.Collections.Generic;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys.Authentication;


namespace AutodictorBL.DataAccess
{
    public class UserService
    {
        private readonly IUsersRepository _repository;



        #region ctor

        public UserService(IUsersRepository repository)
        {
            _repository = repository;
        }

        #endregion




        #region Methode

        public User GetById(int id)
        {
            return _repository.GetById(id);
        }


        public IEnumerable<User> GetAll()
        {
            return _repository.List();
        }

        #endregion
    }
}