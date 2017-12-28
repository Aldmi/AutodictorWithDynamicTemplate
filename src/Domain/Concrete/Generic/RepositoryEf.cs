using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Abstract;
using Domain.Entitys;

namespace Domain.Concrete.Generic
{
    public class RepositoryEf<T> : IRepository<T> where T : EntityBase
    {
        //private readonly ApplicationDbContext _dbContext;

        //public Repository(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public virtual T GetById(int id)
        //{
        //    return _dbContext.Set<T>().Find(id);
        //}

        //public virtual IEnumerable<T> List()
        //{
        //    return _dbContext.Set<T>().AsEnumerable();
        //}

        //public virtual IEnumerable<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        //{
        //    return _dbContext.Set<T>()
        //           .Where(predicate)
        //           .AsEnumerable();
        //}


        //public void Delete(T entity)
        //{
        //    _dbContext.Set<T>().Remove(entity);
        //    _dbContext.SaveChanges();
        //}


        //public void Add(T entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Edit(T entity)
        //{
        //    throw new NotImplementedException();
        //}
        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<T> entity)
        {
            throw new NotImplementedException();
        }
    }
}