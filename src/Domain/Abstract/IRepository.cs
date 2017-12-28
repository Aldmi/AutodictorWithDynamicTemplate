using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Entitys;

namespace Domain.Abstract
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entity); 
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        void Edit(T entity);
    }
}