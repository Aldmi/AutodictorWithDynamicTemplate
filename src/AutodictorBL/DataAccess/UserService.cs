using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public IEnumerable<User> GetAll(Expression<Func<User, bool>> predicate)
        {
            return _repository.List(predicate);
        }

        public IEnumerable<User> GetAllUsersWithRole(Role role)
        {
           return _repository.List(user=> user.Role == role);
        }

        public void AddRange(IEnumerable<User> users)
        {
            _repository.AddRange(users);
        }

        public void Add(User user)
        {
            _repository.Add(user);
        }

        public void Delete(User entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<User, bool>> predicate)
        {
            _repository.Delete(predicate);
        }

        public void DeleteAll()
        {
            _repository.Delete(user=> true);
        }

        public void Edit(User user)
        {
            _repository.Edit(user);
        }


        public async Task DbInitialize()
        {
            await Task.Factory.StartNew(() =>
            {
                string adminLogin = "Админ";
                string adminPassword = "123456";

                var admin = GetAll(user => (user.Role == Role.Администратор) &&
                                           (user.Login == adminLogin)).FirstOrDefault();
                if (admin == null)
                {
                    Add(new User { Login = adminLogin, Password = adminPassword, Role = Role.Администратор });
                }
            });
        }



        #endregion
    }
}