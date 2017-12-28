using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Abstract;
using Domain.Entitys;
using LiteDB;

namespace Domain.Concrete.NoSqlReposutory
{
    public class RepositoryNoSql<T> : IRepository<T> where T : EntityBase
    {
        #region field

        private readonly string _connectionString;

        #endregion





        #region ctor

        public RepositoryNoSql(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion





        public T GetById(int id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var dbContext = db.GetCollection<T>(nameof(T));
                var results = dbContext.FindById(id);
                return results;
            }         
        }



        public IEnumerable<T> List()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                // Get a collection (or create, if doesn't exist)
                var dbContext = db.GetCollection<T>(nameof(T));
                var results = dbContext.FindAll().ToList();
                return results;
            }
        }


        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var dbContext = db.GetCollection<T>(nameof(T));
                var results = dbContext.Find(predicate).ToList();
                return results;
            }
        }


        public void Add(T entity)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                // Get a collection (or create, if doesn't exist)
                var dbContext = db.GetCollection<T>(nameof(T));
                dbContext.Insert(entity);
                dbContext.EnsureIndex(x => x.Id);
            }
        }


        public void AddRange(IEnumerable<T> entity)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                // Get a collection (or create, if doesn't exist)
                var dbContext = db.GetCollection<T>(nameof(T));
                dbContext.Insert(entity);
                dbContext.EnsureIndex(x => x.Id);
            }
        }



        public void Delete(T entity)
        {
            //using (var db = new LiteDatabase(_connectionString))
            //{
            //    // Get a collection (or create, if doesn't exist)
            //    var dbContext = db.GetCollection<T>(nameof(T));

            //    //Insert++++++++++++++++++++++
            //    //foos.Insert(foo);
            //    //foos.EnsureIndex(x => x.Name);


            //    //Insert List+++++++++++++++++
            //    //dbContext.Insert(changeFoos);
            //    //dbContext.EnsureIndex(x => x.TimeStamp);


            //    //View+++++++++++++++++++++++
            //    var results = dbContext.FindAll().ToList();
            //    return results;

            //    //DELETE++++++++++++++++++++++
            //    //foos.Delete(f => true);
            //    //results = foos.FindAll().ToList();
            //    //count = foos.Count();
            //}
        }


        public void Delete(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var dbContext = db.GetCollection<T>(nameof(T));
                dbContext.Delete(predicate);
            }

        }


        public void Edit(T entity)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var dbContext = db.GetCollection<T>(nameof(T));
                dbContext.Update(entity);
            }
        }
    }
}