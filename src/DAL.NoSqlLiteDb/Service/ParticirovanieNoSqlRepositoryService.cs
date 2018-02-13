using System;
using DAL.Abstract.Abstract;
using DAL.Abstract.Entitys;
using DAL.NoSqlLiteDb.Repository;

namespace DAL.NoSqlLiteDb.Service
{
    /// <summary>
    /// Партицирует NoSql репозитории по датам.
    /// Каждые новые сутки идет запись в новый файл БД.
    /// </summary>
    public class ParticirovanieNoSqlRepositoryService<T> : IParticirovanieNoSqlService<T> where T : EntityBase
    {
        private const string BaseFileName = @"NoSqlDb\Main_";


        public IGenericDataRepository<T> GetRepositoryOnCurrentDay()
        {
            var postFix = DateTime.Now.Date.ToString("ddMMyyyy") +".db";
            var connection = BaseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }


        public IGenericDataRepository<T> GetRepositoryOnYesterdayDay()
        {
            var postFix = DateTime.Now.AddDays(-1).Date.ToString("ddMMyyyy") + ".db";
            var connection = BaseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }


        public IGenericDataRepository<T> GetRepositoryOnDay(DateTime date)
        {
            var postFix = date.ToString("ddMMyyyy") + ".db";
            var connection = BaseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }
    }
}
