using System;
using DAL.Abstract.Abstract;
using DAL.Abstract.Entitys;
using DAL.NoSqlLiteDb.Repository;

namespace DAL.NoSqlLiteDb.Service
{
    public interface IParticirovanieNoSqlService<T> where T : EntityBase
    {
        IGenericDataRepository<T> GetRepositoryOnCurrentDay();
        IGenericDataRepository<T> GetRepositoryOnYesterdayDay();
        IGenericDataRepository<T> GetRepositoryOnDay(DateTime date);

    }
}