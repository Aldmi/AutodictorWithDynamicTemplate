using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Abstract.Abstract;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys.Authentication;
using DAL.NoSqlLiteDb.Service;

namespace DAL.NoSqlLiteDb.Repository
{
    public class NoSqlUsersRepository : IUsersRepository
    {
        private IGenericDataRepository<User> Repository { get; set; }




        #region ctor

        public NoSqlUsersRepository(string connection)
        {
            Repository = new RepositoryNoSql<User>(connection);
        }

        #endregion



        public User GetById(int id)
        {
            return Repository.GetById(id);
        }

        public IEnumerable<User> List()
        {
            return Repository.List();
        }

        public IEnumerable<User> List(Expression<Func<User, bool>> predicate)
        {
            return Repository.List(predicate);
        }

        public void Add(User entity)
        {
             Repository.Add(entity);
        }

        public void AddRange(IEnumerable<User> entity)
        {
            Repository.AddRange(entity);
        }

        public void Delete(User entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(Expression<Func<User, bool>> predicate)
        {
            Repository.Delete(predicate);

            var hh= Repository.List();
        }

        public void Edit(User entity)
        {
            Repository.Edit(entity);
        }
    }
}