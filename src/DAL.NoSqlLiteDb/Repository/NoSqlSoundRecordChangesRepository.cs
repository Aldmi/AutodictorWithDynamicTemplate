using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Abstract.Abstract;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;


namespace DAL.NoSqlLiteDb.Repository
{
    public class NoSqlSoundRecordChangesRepository : ISoundRecordChangesRepository
    {
        private IGenericDataRepository<SoundRecordChangesDb> Repository { get; }




        #region ctor

        public NoSqlSoundRecordChangesRepository(string connection)
        {
            Repository = new RepositoryNoSql<SoundRecordChangesDb>(connection);
        }

        #endregion




        public SoundRecordChangesDb GetById(int id)
        {
            return Repository.GetById(id);
        }

        public IEnumerable<SoundRecordChangesDb> List()
        {
            return Repository.List();
        }

        public IEnumerable<SoundRecordChangesDb> List(Expression<Func<SoundRecordChangesDb, bool>> predicate)
        {
            return Repository.List();
        }

        public void Add(SoundRecordChangesDb entity)
        {
            Repository.Add(entity);
        }

        public void AddRange(IEnumerable<SoundRecordChangesDb> entity)
        {
            Repository.AddRange(entity);
        }

        public void Delete(SoundRecordChangesDb entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(Expression<Func<SoundRecordChangesDb, bool>> predicate)
        {
            Repository.Delete(predicate);
        }

        public void Edit(SoundRecordChangesDb entity)
        {
            Repository.Edit(entity);
        }
    }
}