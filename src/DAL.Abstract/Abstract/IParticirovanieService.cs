using System;
using DAL.Abstract.Entitys;

namespace DAL.Abstract.Abstract
{
    public interface IParticirovanieService<T> where T : EntityBase
    {
        IGenericDataRepository<T> GetRepositoryOnCurrentDay();
        IGenericDataRepository<T> GetRepositoryOnYesterdayDay();
        IGenericDataRepository<T> GetRepositoryOnDay(DateTime date);
    }
}